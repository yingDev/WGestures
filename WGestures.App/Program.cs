using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
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
        private static Mutex mutext;

        private static GestureParser gestureParser;

        private static PlistConfig config;
        private static CanvasWindowGestureView gestureView;

        private static readonly List<IDisposable> componentsToDispose = new List<IDisposable>();

        private static SettingsFormController settingsFormController;

        private static bool isFirstRun;
        private static JsonGestureIntentStore intentStore;
        private static Win32GestrueIntentFinder intentFinder;

        [STAThread]
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new DetailedConsoleListener());

            if (IsDuplicateInstance()) return;
            AppWideInit();

            try
            {
                //加载配置文件，如果文件不存在或损坏，则加载默认配置文件
                LoadFailSafeConfigFile();
                CheckAndDoFirstRunStuff();
                SyncAutoStartState();

                ConfigureComponents();
                StartParserThread();

                //显示托盘图标
                ShowTrayIcon();
            }
            catch (Exception e)
            {
                ShowFatalError(e);
            }
            finally { Dispose(); }
        }

        private static void StartParserThread()
        {
            new Thread(() =>
            {
                try
                {
                    gestureParser.Start();
                }
                catch (Exception e)
                {
                    ShowFatalError(e);
                }
            }) {Name = "Parser线程", IsBackground = false}.Start();
        }

        private static bool IsDuplicateInstance()
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

        private static void ShowFatalError(Exception e)
        {
            var frm = new ErrorForm() {Text = Application.ProductName};
            frm.ErrorText = e.ToString();
            frm.ShowDialog();
            Environment.Exit(1);
        }

        private static void CheckAndDoFirstRunStuff()
        {
            //是否是第一次运行
            isFirstRun = config.Get(ConfigKeys.IsFirstRun, false);

            if (isFirstRun)
            {
                ImportPrevousVersion();

                config.Set(ConfigKeys.IsFirstRun, false);
                config.Save();
            }

            if (isFirstRun)
            {
                //ShowQuickStartGuide();
                Warning360Safe();
            }
        }

        private static void AppWideInit()
        {
            Application.EnableVisualStyles();
            Native.SetProcessDPIAware();

            Thread.CurrentThread.IsBackground = true;
            Thread.CurrentThread.Name = "入口线程";


            SetWorkingSet(null, null);
            SystemEvents.DisplaySettingsChanged += SetWorkingSet;
        }

        private static void SetWorkingSet(object sender, EventArgs e)
        {
            using (var proc = Process.GetCurrentProcess())
            {
                //高优先级
                proc.PriorityClass = ProcessPriorityClass.RealTime;

                //工作集
                var screenBounds = Screen.GetBounds(Point.Empty);
                var screenArea = screenBounds.Width * screenBounds.Height;
                var min = screenArea * 4 + 1024 * 1024 * 10;
                var max = min * 1.5f;
                Debug.WriteLine("SetWorkingSet: min=" + min + "; max=" + (int)max);

                Native.SetProcessWorkingSetSize(new IntPtr(proc.Id), min, (int)max);//按屏幕大小来预留工作集
            }
        }


        private static void LoadFailSafeConfigFile()
        {
            if (!File.Exists(AppSettings.ConfigFilePath))
            {
                File.Copy(string.Format("{0}/defaults/config.plist", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.ConfigFilePath);
            }
            if (!File.Exists(AppSettings.GesturesFilePath))
            {
                File.Copy(string.Format("{0}/defaults/gestures.json", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.GesturesFilePath);
            }

            //如果文件损坏，则替换。
            try
            {
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
                intentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath);

                if (config.FileVersion != AppSettings.GesturesFileVersion ||
                intentStore.FileVersion != AppSettings.ConfigFileVersion)
                {
                    throw new Exception("配置文件版本不正确");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("加载配置文件出错："+e);

                File.Delete(AppSettings.GesturesFilePath);
                File.Copy(string.Format("{0}/defaults/gestures.json", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.GesturesFilePath);

                intentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath);
            }

        }


        private static void ImportPrevousVersion()
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

        private static void ConfigureComponents()
        {

            #region Create Components

            intentFinder = new Win32GestrueIntentFinder(intentStore);
            var pathTracker = new Win32MousePathTracker2();
            gestureParser = new GestureParser(pathTracker, intentFinder);

            gestureView = new CanvasWindowGestureView(gestureParser);

            componentsToDispose.Add(gestureParser);
            componentsToDispose.Add(gestureView);
            componentsToDispose.Add(pathTracker);
            #endregion

            #region pathTracker
            //pathTracker.GestureButton = (Win32MousePathTracker2.GestureButtons)config.Get(ConfigKeys.PathTrackerGestureButton, 0);
            pathTracker.InitialValidMove = config.Get(ConfigKeys.PathTrackerInitialValidMove, 4);
            pathTracker.StayTimeout = config.Get(ConfigKeys.PathTrackerStayTimeout, true);
            pathTracker.StayTimeoutMillis = config.Get(ConfigKeys.PathTrackerStayTimeoutMillis, 500);
            pathTracker.InitialStayTimeout = config.Get(ConfigKeys.PathTrackerInitialStayTimeout, true);
            pathTracker.InitialStayTimeoutMillis = config.Get(ConfigKeys.PathTrackerInitialStayTimoutMillis, 150);

            pathTracker.RequestPauseResume += paused => menuItem_pause_Click(null,EventArgs.Empty);

            #endregion

            #region gestureView
            gestureView.ShowPath = config.Get(ConfigKeys.GestureViewShowPath, true);
            gestureView.ShowCommandName = config.Get(ConfigKeys.GestureViewShowCommandName, true);
            gestureView.ViewFadeOut = config.Get(ConfigKeys.GestureViewFadeOut, true);
            gestureView.PathMainColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewMainPathColor, gestureView.PathMainColor.ToArgb()));
            gestureView.PathAlternativeColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewAlternativePathColor, gestureView.PathAlternativeColor.ToArgb()));
            gestureView.PathMiddleBtnMainColor = Color.FromArgb(config.Get(ConfigKeys.GestureViewMiddleBtnMainColor, gestureView.PathMiddleBtnMainColor.ToArgb()));

            #endregion


            #region GestureParser
            gestureParser.DisableInFullScreenMode = config.Get(ConfigKeys.GestureParserDisableInFullScreenMode, false);
            #endregion

        }

        private static void ShowTrayIcon()
        {
            using (var notifyIcon = CreateNotifyIcon())
            {
                if (isFirstRun)
                {
                    notifyIcon.ShowBalloonTip(1000 * 10, "WGstures在这里", "双击图标打开设置，右击查看菜单\n鼠标 左键+中键 随时暂停/继续手势", ToolTipIcon.Info);
                }

                notifyIcon.DoubleClick += (sender, args) => ShowSettings();
                //notifyIcon.Click += (sender, args) => menuItem_pause_Click(null, EventArgs.Empty);

                //是否检查更新
                if (!config.Get<bool?>(ConfigKeys.AutoCheckForUpdate).HasValue || config.Get<bool>(ConfigKeys.AutoCheckForUpdate))
                {
                    var checkForUpdateTimer = new Timer { Interval = Constants.AutoCheckForUpdateInterval };

                    checkForUpdateTimer.Tick += (sender, args) =>
                    {
                        ScheduledUpdateCheck(sender, notifyIcon);
                    };
                    checkForUpdateTimer.Start();
                }

                Application.Run();
            }
        }

        #region event handlers
        private static void menuItem_settings_Click(object sender, EventArgs eventArgs)
        {
            ShowSettings();
        }

        private static void menuItem_pause_Click(object sender, EventArgs eventArgs)
        {
            if (gestureParser.IsPaused)
            {
                gestureParser.Resume();
            }
            else
            {
                gestureParser.Pause();
            }
        }

        private static void menuItem_exit_Click(object sender, EventArgs e)
        {
            gestureParser.Stop();
            Application.ExitThread();
        }
        #endregion

        //仅在启动一段时间后检查一次更新，
        private static void ScheduledUpdateCheck(object sender, NotifyIcon tray)
        {
            var timer = sender as Timer;
            timer.Stop();
            timer.Dispose();
            timer = null;

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
                        }
                    };
                    tray.ShowBalloonTip(1000 * 15, Application.ProductName + "新版本可用!", "版本:" + info.Version + "\n" + whatsNew, ToolTipIcon.Info);
                }

                checker.Dispose();
                checker = null;

                GC.Collect();
            };

#if DEBUG

            checker.ErrorHappened += e =>
            {
                Debug.WriteLine("Program.ScheduledUpdateCheck Error:" + e.Message);
                checker.Dispose();
                checker = null;

                GC.Collect();
            };
#endif

            checker.CheckAsync();
        }

        private static void ShowSettings()
        {
            if (settingsFormController != null)
            {
                settingsFormController.BringToFront();
                return;
            }
            using (settingsFormController = new SettingsFormController(config, gestureParser,
                (Win32MousePathTracker2)gestureParser.PathTracker, intentStore, gestureView))
            {
                //进程如果优先为Hight，设置窗口上执行手势会响应非常迟钝（原因不明）
                using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.Normal;
                settingsFormController.ShowDialog();
                using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.High;
            }

            //settingsFormController.Dispose();
            settingsFormController = null;
            //GC.Collect();

            //Native.EmptyWorkingSet(Process.GetCurrentProcess().Handle);

        }

        //用配置信息去同步自启动
        private static void SyncAutoStartState()
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

        private static void ShowQuickStartGuide()
        {

            var t = new Thread(() =>
            {
                using (var proc = Process.GetCurrentProcess())
                {
                    proc.PriorityClass = ProcessPriorityClass.Normal;
                }

                Form frm;
                using (frm = new QuickStartGuideForm())
                {
                    Application.Run(frm);
                }

                frm = null;
                GC.Collect();

                using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.High;

            }) { IsBackground = true };

            t.SetApartmentState(ApartmentState.STA);

            t.Start();

        }

        private static NotifyIcon CreateNotifyIcon()
        {
            var notifyIcon = new NotifyIcon();

            var contextMenu1 = new ContextMenu();

            var menuItem_exit = new MenuItem() { Text = "退出" };
            menuItem_exit.Click += menuItem_exit_Click;

            var menuItem_pause = new MenuItem() { Text = "暂停 (左键 + 中键)" };
            menuItem_pause.Click += menuItem_pause_Click;

            var menuItem_settings = new MenuItem() { Text = "设置" };
            menuItem_settings.Click += menuItem_settings_Click;

            var menuItem_showQuickStart = new MenuItem() { Text = "快速入门" };
            menuItem_showQuickStart.Click += (sender, args) => ShowQuickStartGuide();

            contextMenu1.MenuItems.AddRange(new[] { menuItem_pause, menuItem_settings, new MenuItem("-"), menuItem_showQuickStart, menuItem_exit });

            notifyIcon.Icon = Resources.trayIcon;
            notifyIcon.Text = Application.ProductName;
            notifyIcon.ContextMenu = contextMenu1;
            notifyIcon.Visible = true;

            gestureParser.StateChanged += state =>
            {
                var mouseSwapped = Native.GetSystemMetrics(Native.SystemMetric.SM_SWAPBUTTON) != 0;
                if (state == GestureParser.State.PAUSED)
                {
                    menuItem_pause.Text = string.Format("继续 ({0}键 + 中键)",mouseSwapped ? "右" : "左");
                    notifyIcon.Icon = Resources.trayIcon_bw;
                }
                else
                {
                    menuItem_pause.Text = string.Format("暂停 ({0}键 + 中键)", mouseSwapped ? "右" : "左");
                    notifyIcon.Icon = Resources.trayIcon;

                }
            };

            return notifyIcon;

        }

        private static void Warning360Safe()
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

        private static void Dispose()
        {
            try
            {
                SystemEvents.DisplaySettingsChanged -= SetWorkingSet;

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
            }


        }
    }
}
