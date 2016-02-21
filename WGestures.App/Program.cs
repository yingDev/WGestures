using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WGestures.App.Gui.Windows;
using WGestures.App.Migrate;
using WGestures.App.Properties;
using WGestures.Common;
using WGestures.Common.Config.Impl;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common.Product;
using WGestures.Core;
using WGestures.Core.Impl.Windows;
using WGestures.Core.Persistence.Impl;
using WGestures.Core.Persistence.Impl.Windows;
using WGestures.View.Impl.Windows;
using Timer = System.Windows.Forms.Timer;

namespace WGestures.App
{
    static class Program
    {
        static Mutex mutext;
        static GestureParser gestureParser;

        static PlistConfig config;
        static CanvasWindowGestureView gestureView;

        static readonly IList<IDisposable> componentsToDispose = new List<IDisposable>();
        static SettingsFormController settingsFormController;

        static bool isFirstRun;
        static JsonGestureIntentStore intentStore;
        static Win32GestrueIntentFinder intentFinder;

        static NotifyIcon trayIcon;
        static GlobalHotKeyManager hotkeyMgr;
        
        //for adding hotkey
        static MenuItem menuItem_pause;

        [STAThread]
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new DetailedConsoleListener());

            if (IsDuplicateInstance())
            {
                //让主实例打开`设置`
                PostIpcCmd("ShowSettings");
                return;
            }

            AppWideInit();

            try
            {
                //加载配置文件，如果文件不存在或损坏，则加载默认配置文件
                LoadFailSafeConfigFile();
                
                SyncAutoStartState();

                ConfigureComponents();
                StartParserThread();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();

                //显示托盘图标
                ShowTrayIcon();

                //监听IPC消息
                StartIpcPipe();

                CheckAndDoFirstRunStuff();
                Application.Run();

            }
#if !DEBUG
            catch (Exception e)
            {
                ShowFatalError(e);
            }
#endif
            finally { Dispose(); }

            

        }

        //TODO: refactor out
        static void StartIpcPipe()
        {
            //todo: impl a set of IPC APIs to perform some cmds. eg.Pause/Resume, Quit...
            //todo: Temporay IPC mechanism.
            var synCtx = new WindowsFormsSynchronizationContext();//note: won't work with `SynchronizationContext.Current`
            var pipeThread = new Thread(() =>
            {
                while (true)
                {
                    using (var server = new System.IO.Pipes.NamedPipeServerStream("WGestures_IPC_API"))
                    {
                        server.WaitForConnection();

                        Debug.WriteLine("Clien Connected");
                        using (var reader = new StreamReader(server))
                        {
                            var cmd = reader.ReadLine();
                            Debug.WriteLine("Pipe CMD=" + cmd);

                            if (cmd == "ShowSettings")
                            {
                                synCtx.Post((s) =>
                                {
                                    Debug.WriteLine("Thread=" + Thread.CurrentThread.ManagedThreadId);
                                    ShowSettings();
                                }, null);
                            }
                        }
                    }
                }
            }, maxStackSize: 1) { IsBackground = true };
            pipeThread.Start();
        }

        static void PostIpcCmd(string cmd)
        {
            using (var pipeClient = new System.IO.Pipes.NamedPipeClientStream("WGestures_IPC_API"))
            {
                pipeClient.Connect();
                using (var writer = new StreamWriter(pipeClient) { AutoFlush = true })
                {
                    writer.WriteLine(cmd);
                }
            }
        }

        static void StartParserThread()
        {
            new Thread(() =>
            {
#if DEBUG
                gestureParser.Start();
#else
                try
                {
                    gestureParser.Start();
                }
                catch (Exception e)
                {
                    ShowFatalError(e);
                }
#endif
            }, maxStackSize: 1) {Name = "Parser线程", Priority = ThreadPriority.Highest, IsBackground = false}.Start();
        }

        static bool IsDuplicateInstance()
        {
            bool createdNew;
            mutext = new Mutex(true, Constants.Identifier, out createdNew);
            if (!createdNew)
            {
                mutext.Close();
                return true;
            }
            return false;
        }

        static void ShowFatalError(Exception e)
        {
            var frm = new ErrorForm() {Text = Application.ProductName};
            frm.ErrorText = e.ToString();
            frm.ShowDialog();
            Environment.Exit(1);
        }

        static void CheckAndDoFirstRunStuff()
        {
            //是否是第一次运行
             var maybeFirstRun = config.Get<bool?>(ConfigKeys.IsFirstRun);
            isFirstRun = (!maybeFirstRun.HasValue || maybeFirstRun.Value);

            if (isFirstRun)
            {
                //默认值
                config.Set(ConfigKeys.GestureParserEnableHotCorners, true);

                ImportPrevousVersion();

                //强制值
                config.Set(ConfigKeys.IsFirstRun, false);
                config.Set(ConfigKeys.AutoCheckForUpdate, true);
                config.Set(ConfigKeys.AutoStart, true);
                
                config.Save();
            
                ShowQuickStartGuide(isFirstRun: true);
                Warning360Safe();
            }
        }

        static void AppWideInit()
        {
            Application.EnableVisualStyles();
            Native.SetProcessDPIAware();

            Thread.CurrentThread.IsBackground = false;
            Thread.CurrentThread.Name = "入口线程";

            using (var proc = Process.GetCurrentProcess())
            {
                //高优先级
                proc.PriorityClass = ProcessPriorityClass.High;
            }

            hotkeyMgr = new GlobalHotKeyManager();
        }
        
        static void LoadFailSafeConfigFile()
        {
            if (!File.Exists(AppSettings.ConfigFilePath))
            {
                File.Copy(string.Format("{0}/defaults/config.plist", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.ConfigFilePath);
            }
            if (!File.Exists(AppSettings.GesturesFilePath))
            {
                File.Copy(string.Format("{0}/defaults/gestures.wg2", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.GesturesFilePath);
            }
            
            try
            { //如果文件损坏，则替换。
                config = new PlistConfig(AppSettings.ConfigFilePath);
            }
            catch (Exception)
            {
                Debug.WriteLine("Program.Main: config文件损坏！");
                File.Delete(AppSettings.ConfigFilePath);
                File.Copy(string.Format("{0}/defaults/config.plist", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.ConfigFilePath);

                config = new PlistConfig(AppSettings.ConfigFilePath);
            }
            
            try
            {
                intentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);

                if (config.FileVersion != AppSettings.ConfigFileVersion ||
                intentStore.FileVersion != AppSettings.GesturesFileVersion)
                {
                    throw new Exception("配置文件版本不正确");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("加载配置文件出错："+e);

                File.Delete(AppSettings.GesturesFilePath);
                File.Copy(string.Format("{0}/defaults/gestures.wg", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.GesturesFilePath);

                intentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);
            }

        }


        static void ImportPrevousVersion()
        {
            try
            {
                //导入先前版本
                var prevConfigAndGestures = MigrateService.ImportPrevousVersion();
                if (prevConfigAndGestures == null) return;

                intentStore.Import(prevConfigAndGestures.GestureIntentStore);
                config.Import(prevConfigAndGestures.Config);

                intentStore.Save();
            }
            catch (MigrateException e)
            {
                //ignore
#if DEBUG
                throw;
#endif
            }
        }

        static void ConfigureComponents()
        {
#region Create Components
            intentFinder = new Win32GestrueIntentFinder(intentStore);
            var pathTracker = new Win32MousePathTracker2();
            gestureParser = new GestureParser(pathTracker, intentFinder);

            gestureView = new CanvasWindowGestureView(gestureParser);

            componentsToDispose.Add(gestureParser);
            componentsToDispose.Add(gestureView);
            componentsToDispose.Add(pathTracker);
            componentsToDispose.Add(hotkeyMgr);
#endregion

#region pathTracker
            pathTracker.DisableInFullscreen = config.Get(ConfigKeys.PathTrackerDisableInFullScreen, true);
            pathTracker.PreferWindowUnderCursorAsTarget = config.Get(ConfigKeys.PathTrackerPreferCursorWindow, false);
            pathTracker.TriggerButton = (GestureTriggerButton)config.Get(ConfigKeys.PathTrackerTriggerButton, GestureTriggerButton.Right);
            pathTracker.InitialValidMove = config.Get(ConfigKeys.PathTrackerInitialValidMove, 4);
            pathTracker.StayTimeout = config.Get(ConfigKeys.PathTrackerStayTimeout, true);
            pathTracker.StayTimeoutMillis = config.Get(ConfigKeys.PathTrackerStayTimeoutMillis, 500);
            pathTracker.InitialStayTimeout = config.Get(ConfigKeys.PathTrackerInitialStayTimeout, true);
            pathTracker.InitialStayTimeoutMillis = config.Get(ConfigKeys.PathTrackerInitialStayTimoutMillis, 150);
            pathTracker.RequestPauseResume += paused => menuItem_pause_Click(null,EventArgs.Empty);
            //pathTracker.RequestShowHideTray += ToggleTrayIconVisibility ;
#endregion

#region gestureView
            gestureView.ShowPath = config.Get(ConfigKeys.GestureViewShowPath, true);
            gestureView.ShowCommandName = config.Get(ConfigKeys.GestureViewShowCommandName, true);
            gestureView.ViewFadeOut = config.Get(ConfigKeys.GestureViewFadeOut, true);
            gestureView.PathMainColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewMainPathColor, gestureView.PathMainColor.ToArgb()));
            gestureView.PathAlternativeColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewAlternativePathColor, gestureView.PathAlternativeColor.ToArgb()));
            gestureView.PathMiddleBtnMainColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewMiddleBtnMainColor, gestureView.PathMiddleBtnMainColor.ToArgb()));
            gestureView.PathXBtnMainColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewXBtnPathColor, gestureView.PathXBtnMainColor.ToArgb()));
            #endregion

            #region GestureParser
            gestureParser.EnableHotCorners = config.Get(ConfigKeys.GestureParserEnableHotCorners, true);
            gestureParser.Enable8DirGesture = config.Get(ConfigKeys.GestureParserEnable8DirGesture, true);
            gestureParser.EnableRubEdge = config.Get(ConfigKeys.GestureParserEnableRubEdges, true);
#endregion



            //HOt key
            hotkeyMgr.HotKeyPreview += HotkeyMgr_HotKeyPreview;

            var pauseHotKey = config.Get<byte[]>(ConfigKeys.PauseResumeHotKey, null);
            if (pauseHotKey != null)
            {
                var hotkey = GlobalHotKeyManager.HotKey.FromBytes(pauseHotKey);

                try
                {
                    hotkeyMgr.RegisterHotKey(ConfigKeys.PauseResumeHotKey, hotkey, null);
                }catch(InvalidOperationException e)
                {
                    Debug.WriteLine(e);

                    //ignore for now ?
                }
                
            }
        }

        static bool HotkeyMgr_HotKeyPreview(GlobalHotKeyManager mgr, string id, GlobalHotKeyManager.HotKey hk)
        {
            if(id == ConfigKeys.PauseResumeHotKey)
            {
                Debug.WriteLine("HotKey Pressed: " + hk);
                TogglePause();
                menuItem_pause.Text = string.Format("{0} ({1})", gestureParser.IsPaused ? "继续" : "暂停" ,hk.ToString());

                return true; //Handled
            }

            return false;
        }

        static void TogglePause()
        {
            gestureParser.TogglePause();
        }

        static void ShowTrayIcon()
        {
                trayIcon = CreateNotifyIcon();
                EventHandler handleBalloon = (sender, args) =>
                {
                    var timer = new Timer { Interval = 1000 };
                    timer.Tick += (sender_1, args_1) =>
                    {
                        timer.Stop();
                        trayIcon.Visible = config.Get(ConfigKeys.TrayIconVisible, true);
                    };
                    timer.Start();
                };

                trayIcon.BalloonTipClosed += handleBalloon;
                trayIcon.BalloonTipClicked += handleBalloon;
                trayIcon.DoubleClick += (sender, args) => ShowSettings();
            
                if (isFirstRun)
                {
                    trayIcon.ShowBalloonTip(1000 * 10, "WGstures在这里", "双击图标打开设置，右击查看菜单\n鼠标 左键+中键 随时暂停/继续手势", ToolTipIcon.Info);
                }
                //else
                //{
                    //var showIcon = config.Get<bool?>(ConfigKeys.TrayIconVisible);
                    //if (showIcon.HasValue && !showIcon.Value) //隐藏
                    //{
                    //   trayIcon.ShowBalloonTip(3000, "WGestures后台运行中", "图标将自动隐藏。\n(按 Shift-左键-中键 切换显示/隐藏状态)", ToolTipIcon.Info);
                    //}
                //}
                //是否检查更新
                if (!config.Get<bool?>(ConfigKeys.AutoCheckForUpdate).HasValue || config.Get<bool>(ConfigKeys.AutoCheckForUpdate))
                {
                    var checkForUpdateTimer = new Timer { Interval = Constants.AutoCheckForUpdateInterval };

                    checkForUpdateTimer.Tick += (sender, args) =>
                    {
                        checkForUpdateTimer.Stop();
                        ScheduledUpdateCheck(sender, trayIcon);

                    };
                    checkForUpdateTimer.Start();
                }
        }

#region event handlers
        static void menuItem_settings_Click(object sender, EventArgs eventArgs)
        {
            ShowSettings();
        }

        static void menuItem_pause_Click(object sender, EventArgs eventArgs)
        {
            TogglePause();
        }

        static void menuItem_exit_Click(object sender, EventArgs e)
        {
            gestureParser.Stop();
            Application.ExitThread();
            trayIcon.Dispose();
        }


#endregion

        //仅在启动一段时间后检查一次更新，
        static void ScheduledUpdateCheck(object sender, NotifyIcon tray)
        {
            if (!config.Get<bool>(ConfigKeys.AutoCheckForUpdate)) return;
            
            var checker = new VersionChecker(AppSettings.CheckForUpdateUrl);
            checker.Finished += info =>
            {
                var whatsNew = info.WhatsNew.Length > 50 ? info.WhatsNew.Substring(0, 50) : info.WhatsNew;
                
                if (info.Version != Application.ProductVersion)
                {
                    tray.BalloonTipClicked += (o, args) =>
                    {
                        if (info.Version == Application.ProductVersion) return;
                        using (var frm = new UpdateInfoForm(ConfigurationManager.AppSettings.Get(Constants.ProductHomePageAppSettingKey), info))
                        {
                            frm.ShowDialog();
                            tray.Visible = config.Get(ConfigKeys.TrayIconVisible, true);
                        }
                    };
                    if (!tray.Visible)
                    {
                        tray.Visible = true;
                    }
                    
                    tray.ShowBalloonTip(1000 * 15, Application.ProductName + "新版本可用!", "版本:" + info.Version + "\n" + whatsNew, ToolTipIcon.Info);
                }

                checker.Dispose();
                checker = null;

                GC.Collect();
            };
            checker.ErrorHappened += e =>
            {
                Debug.WriteLine("Program.ScheduledUpdateCheck Error:" + e.Message);
                checker.Dispose();
                checker = null;

                GC.Collect();
            };

            checker.CheckAsync();
        }

        [Obsolete]
        static void ToggleTrayIconVisibility()
        {            
            //如果图标当前可见， 而config中设置的值是不可见， 则说明是临时显示; 如果不是临时显示， 才需要修改config
            if (!(trayIcon.Visible && !config.Get(ConfigKeys.TrayIconVisible, true)))
            {
                config.Set(ConfigKeys.TrayIconVisible, !trayIcon.Visible);
                config.Save();
            }

            if(trayIcon.Visible)
            {
                trayIcon.ShowBalloonTip(5000, "WGestures图标将隐藏", "(按 Shift + 左键 + 中键 恢复显示)",ToolTipIcon.Info);
             }else
            {
                trayIcon.Visible = true;
            }
        }

        static void ShowSettings()
        {
            if (settingsFormController != null)
            {
                settingsFormController.BringToFront();
                return;
            }
            using (settingsFormController = new SettingsFormController(config, gestureParser,
                (Win32MousePathTracker2)gestureParser.PathTracker, intentStore, gestureView, hotkeyMgr))
            {
                //进程如果优先为Hight，设置窗口上执行手势会响应非常迟钝（原因不明）
               //using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.Normal;
                settingsFormController.ShowDialog();
                //using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.High;
            }
            settingsFormController = null;
        }

        //用配置信息去同步自启动
        static void SyncAutoStartState()
        {
            var fact = AutoStarter.IsRegistered(Constants.Identifier, Application.ExecutablePath);
            var conf = config.Get<bool>(ConfigKeys.AutoStart);

            if (fact == conf && !isFirstRun) return;

            try
            {
                //可能被杀毒软件阻止
                if (conf) AutoStarter.Register(Constants.Identifier, Application.ExecutablePath);
                else
                {
                    AutoStarter.Unregister(Constants.Identifier);
                }
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
            }
        }

        static void ShowQuickStartGuide(bool isFirstRun=false)
        {
            var t = new Thread(() =>
            {
                bool createdNew;
                var mut = new Mutex(true, Constants.Identifier + "QuickStartGuideWindow", out createdNew);
                if (!createdNew) return;
                
                using (var frm = new QuickStartGuideForm())
                {
                    Application.Run(frm);
                    mut.Close();
                }

                if(isFirstRun)
                {
                    //Open again to show settings
                    Process.Start(Application.ExecutablePath);
                }

                GC.Collect();
            }) { IsBackground = true };

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        static string GetPauseResumeHotkeyStringFromConfig()
        {
            var bytes = config.Get<byte[]>(ConfigKeys.PauseResumeHotKey, null);
            if(bytes != null)
            {
                return GlobalHotKeyManager.HotKey.FromBytes(bytes).ToString();
            }

            return "";
        }

        static NotifyIcon CreateNotifyIcon()
        {
            var notifyIcon = new NotifyIcon();

            var contextMenu1 = new ContextMenu();

            var menuItem_exit = new MenuItem() { Text = "退出" };
            menuItem_exit.Click += menuItem_exit_Click;

            menuItem_pause = new MenuItem() { Text = string.Format("暂停 ({0})", GetPauseResumeHotkeyStringFromConfig()) };
            menuItem_pause.Click += menuItem_pause_Click;

            var menuItem_settings = new MenuItem() { Text = "设置" };
            menuItem_settings.Click += menuItem_settings_Click;

            var menuItem_showQuickStart = new MenuItem() { Text = "快速入门" };
            menuItem_showQuickStart.Click += (sender, args) => ShowQuickStartGuide();

            /*var menuItem_toggleTray = new MenuItem() { Text = "隐藏 (Shift + 左键 + 中键)" };
            menuItem_toggleTray.Click += (sender, args) =>
            {
               ToggleTrayIconVisibility();
            };*/

            contextMenu1.MenuItems.AddRange(new[] { /*menuItem_toggleTray, */menuItem_pause, new MenuItem("-"), menuItem_settings,  menuItem_showQuickStart,new MenuItem("-"), menuItem_exit });
            notifyIcon.Icon = Resources.trayIcon;
            notifyIcon.Text = Application.ProductName;
            notifyIcon.ContextMenu = contextMenu1;
            notifyIcon.Visible = true;

            gestureParser.StateChanged += state =>
            {
                var hotKeyStr = GetPauseResumeHotkeyStringFromConfig();
                hotKeyStr = string.IsNullOrEmpty(hotKeyStr) ? "" : string.Format("({0})", hotKeyStr);
                if (state == GestureParser.State.PAUSED)
                {
                    menuItem_pause.Text = "继续 " + hotKeyStr;
                    
                    notifyIcon.Icon = Resources.trayIcon_bw;
                } else
                {
                    menuItem_pause.Text = "暂停 " + hotKeyStr;
                    notifyIcon.Icon = Resources.trayIcon;
                }
            };

            return notifyIcon;
        }

        static void Warning360Safe()
        {
            var proc360 = Process.GetProcessesByName("360Safe");
            var proc360Tray = Process.GetProcessesByName("360Tray");
            
            if(proc360.Length + proc360Tray.Length > 0)
            {
                using (var warn = new Warn360())
                {
                    warn.ShowDialog();
                }
            }
        }

        static void Dispose()
        {
            try
            {
                foreach (var disposable in componentsToDispose)
                {
                    if (disposable != null) disposable.Dispose();
                }

                componentsToDispose.Clear();
                Resources.ResourceManager.ReleaseAllResources();
            }
            finally
            {
                mutext.ReleaseMutex();
                Environment.Exit(1);
            }
        }
    }
}
