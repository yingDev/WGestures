using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WGestures.App.Gui.Windows.CommandViews;
using WGestures.App.Migrate;
using WGestures.Common;
using WGestures.Common.Annotation;
using WGestures.Common.Config;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common.Product;
using WGestures.Core;
using WGestures.Core.Annotations;
using WGestures.Core.Commands.Impl;
using WGestures.Core.Impl.Windows;
using WGestures.Core.Persistence;
using WGestures.Core.Persistence.Impl;
using WGestures.View.Impl.Windows;

namespace WGestures.App.Gui.Windows
{
    internal class SettingsFormController : IDisposable, INotifyPropertyChanged
    {
        private Form _form;

        public bool IsDisposed { get; private set; }

        private IConfig _config;
        private GestureParser _parser;
        private Win32MousePathTracker2 _pathTracker;
        private JsonGestureIntentStore _intentStore;
        private CanvasWindowGestureView _gestureView;

        public SettingsFormController(IConfig config, GestureParser parser,
            Win32MousePathTracker2 pathTracker, JsonGestureIntentStore intentStore,
            CanvasWindowGestureView gestureView)
        {

            _config = config;
            _parser = parser;
            _pathTracker = pathTracker;
            _intentStore = intentStore;
            _gestureView = gestureView;

            #region 初始化支持的命令和命令视图
            SupportedCommands = new Dictionary<string, Type>();
            CommandViewFactory = new CommandViewFactory<CommandViewUserControl>() { EnableCaching = false };

            //Add Command Types
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(DoNothingCommand)), typeof(DoNothingCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(HotKeyCommand)), typeof(HotKeyCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(WebSearchCommand)), typeof(WebSearchCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(WindowControlCommand)), typeof(WindowControlCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(TaskSwitcherCommand)), typeof(TaskSwitcherCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(OpenFileCommand)), typeof(OpenFileCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(SendTextCommand)), typeof(SendTextCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(GotoUrlCommand)), typeof(GotoUrlCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(CmdCommand)), typeof(CmdCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(ScriptCommand)), typeof(ScriptCommand));

            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(PauseWGesturesCommand)), typeof(PauseWGesturesCommand));
            SupportedCommands.Add(NamedAttribute.GetNameOf(typeof(ChangeAudioVolumeCommand)), typeof(ChangeAudioVolumeCommand));



            CommandViewFactory.Register<OpenFileCommand, OpenFileCommandView>();
            CommandViewFactory.Register<DoNothingCommand, GeneralNoParameterCommandView>();
            CommandViewFactory.Register<HotKeyCommand, HotKeyCommandView>();
            CommandViewFactory.Register<GotoUrlCommand, GotoUrlCommandView>();
            CommandViewFactory.Register<PauseWGesturesCommand, GeneralNoParameterCommandView>();
            CommandViewFactory.Register<WebSearchCommand, WebSearchCommandView>();
            CommandViewFactory.Register<WindowControlCommand, WindowControlCommandView>();
            CommandViewFactory.Register<CmdCommand, CmdCommandView>();
            CommandViewFactory.Register<SendTextCommand, SendTextCommandView>();
            CommandViewFactory.Register<TaskSwitcherCommand, TaskSwitcherCommandView>();
            CommandViewFactory.Register<ScriptCommand, ScriptCommandView>();

            CommandViewFactory.Register<ChangeAudioVolumeCommand, GeneralNoParameterCommandView>();



            #endregion

            _form = new SettingsForm(this);
        }


        #region tab1 Config Items

        public IConfig Config { get { return _config; } }
        /// <summary>
        /// 获取或设置是否开机自动运行
        /// </summary>
        public bool AutoStart
        {
            get
            {
                var config = _config.Get<bool>(ConfigKeys.AutoStart);
                return config;
            }
            set
            {
                if (value == AutoStart) return;

                try
                {
                    if (value) AutoStarter.Register(Constants.Identifier, Application.ExecutablePath);
                    else AutoStarter.Unregister(Constants.Identifier);

                    _config.Set(ConfigKeys.AutoStart, value);
                }
                catch (Exception)
                {
#if DEBUG                    
                    throw;
#endif
                }


                OnPropertyChanged("AutoStart");
            }
        }
        /// <summary>
        /// 获取或设置是否自动检查更新
        /// </summary>
        public bool AutoCheckForUpdate
        {
            get { return _config.Get<bool>(ConfigKeys.AutoCheckForUpdate); }
            set
            {
                if (value == AutoCheckForUpdate) return;

                _config.Set(ConfigKeys.AutoCheckForUpdate, value);

                OnPropertyChanged("AutoCheckForUpdate");
            }
        }
        /// <summary>
        /// 获取或设置哪一个鼠标按键作为手势键
        /// </summary>
        public string PathTrackerTriggerButton
        {
            get
            {
                switch (_pathTracker.TriggerButton)
                {
                    case Win32MousePathTracker2.GestureTriggerButton.Middle:
                        return "中键";
                    case Win32MousePathTracker2.GestureTriggerButton.Right:
                        return Native.IsMouseButtonSwapped() ? "左键" : "右键";
                    case Win32MousePathTracker2.GestureTriggerButton.Middle | Win32MousePathTracker2.GestureTriggerButton.Right:
                        return (Native.IsMouseButtonSwapped() ? "左键" : "右键") + "和中键";
                }
                throw new InvalidOperationException("wtf？ 怎么会到这里?");
            }

            set
            {
                if (value == PathTrackerTriggerButton) return;
                if (value.Contains("左键") || value.Contains("右键"))
                {
                    _pathTracker.TriggerButton = Win32MousePathTracker2.GestureTriggerButton.Right;
                    if (value.Contains("中键"))
                    {
                        _pathTracker.TriggerButton |= Win32MousePathTracker2.GestureTriggerButton.Middle;
                    }
                }
                else
                {
                    _pathTracker.TriggerButton = Win32MousePathTracker2.GestureTriggerButton.Middle;
                }

                _config.Set(ConfigKeys.PathTrackerTriggerButton, (int)_pathTracker.TriggerButton);

                OnPropertyChanged("PathTrackerTriggerButton");
            }
        }

        public bool GestureParserEnable8DirGesture
        {
            get { return _parser.Enable8DirGesture; }
            set
            {
                if (value == _parser.Enable8DirGesture) return;
                _parser.Enable8DirGesture = value;
                _config.Set(ConfigKeys.GestureParserEnable8DirGesture, value);
                OnPropertyChanged("GestureParserEnable8DirGesture");
            }
        }

        /// <summary>
        /// 获取或设置出示有效移动距离
        /// </summary>
        public int PathTrackerInitialValidMove
        {
            get { return _pathTracker.InitialValidMove; }
            set
            {
                if (value == PathTrackerInitialValidMove) return;

                _pathTracker.InitialValidMove = value;
                _config.Set(ConfigKeys.PathTrackerInitialValidMove, _pathTracker.InitialValidMove);

                OnPropertyChanged("PathTrackerInitialValidMove");
            }
        }

        public bool PathTrackerInitialStayTimeout
        {
            get { return _pathTracker.InitialStayTimeout; }
            set
            {
                if (value == PathTrackerInitialStayTimeout) return;

                _pathTracker.InitialStayTimeout = value;
                _config.Set(ConfigKeys.PathTrackerInitialStayTimeout, value);

                OnPropertyChanged("PathTrackerInitialStayTimeout");
            }
        }

        public int PathTrackerInitalStayTimeoutMillis
        {
            get { return _pathTracker.InitialStayTimeoutMillis; }
            set
            {
                if (value == PathTrackerStayTimeoutMillis) return;

                _pathTracker.InitialStayTimeoutMillis = value;
                _config.Set(ConfigKeys.PathTrackerInitialStayTimoutMillis, value);

                OnPropertyChanged("PathTrackerInitalStayTimeoutMillis");
            }
        }

        public bool PathTrackerDisableInFullScreen
        {
            get { return _pathTracker.DisableInFullscreen; }
            set
            {
                if (value == PathTrackerDisableInFullScreen) return;

                _pathTracker.DisableInFullscreen = value;
                _config.Set(ConfigKeys.PathTrackerDisableInFullScreen, value);

                OnPropertyChanged("PathTrackerDisableInFullScreen");
            }
        }

        /// <summary>
        /// 获取或设置鼠标是否允许停留超时
        /// </summary>
        public bool PathTrackerStayTimeout
        {
            get { return _pathTracker.StayTimeout; }
            set
            {
                if (value == PathTrackerStayTimeout) return;

                _pathTracker.StayTimeout = value;
                _config.Set(ConfigKeys.PathTrackerStayTimeout, _pathTracker.StayTimeout);

                OnPropertyChanged("PathTrackerStayTimeout");
            }
        }
        /// <summary>
        /// 获取或设置停留超时的时间（毫秒）
        /// </summary>
        public int PathTrackerStayTimeoutMillis
        {
            get { return _pathTracker.StayTimeoutMillis; }
            set
            {
                if (value == PathTrackerStayTimeoutMillis) return;

                _pathTracker.StayTimeoutMillis = value;
                _config.Set(ConfigKeys.PathTrackerStayTimeoutMillis, _pathTracker.StayTimeoutMillis);

                OnPropertyChanged("PathTrackerStayTimeoutMillis");
            }
        }

        public bool PathTrackerPreferCursorWindow
        {
            get 
            {
                return _pathTracker.PreferWindowUnderCursorAsTarget;
            }
            set 
            {
                if (value == PathTrackerPreferCursorWindow) return;
                _pathTracker.PreferWindowUnderCursorAsTarget = value;
                _config.Set(ConfigKeys.PathTrackerPreferCursorWindow, value);

                OnPropertyChanged("PathTrackerPreferCursorWindow");
            }
        }
        /// <summary>
        /// 获取或设置是否显示手势路径
        /// </summary>
        public bool GestureViewShowPath
        {
            get { return _gestureView.ShowPath; }
            set
            {
                if (value == GestureViewShowPath) return;

                _gestureView.ShowPath = value;
                _config.Set(ConfigKeys.GestureViewShowPath, value);

                OnPropertyChanged("GestureViewShowPath");
            }
        }
        /// <summary>
        /// 获取或设置是否显示手示意图名称
        /// </summary>
        public bool GestureViewShowCommandName
        {
            get { return _gestureView.ShowCommandName; }
            set
            {
                if (value == GestureViewShowCommandName) return;

                _gestureView.ShowCommandName = value;
                _config.Set(ConfigKeys.GestureViewShowCommandName, value);

                OnPropertyChanged("GestureViewShowCommandName");
            }
        }
        /// <summary>
        /// 获取或设置是否在手势执行后视图淡出
        /// </summary>
        public bool GestureViewFadeOut
        {
            get { return _gestureView.ViewFadeOut; }
            set
            {
                if (value == GestureViewFadeOut) return;

                _gestureView.ViewFadeOut = value;
                _config.Set(ConfigKeys.GestureViewFadeOut, value);

                OnPropertyChanged("GestureViewFadeOut");
            }
        }
        /// <summary>
        /// 获取或设置手势被识别时轨迹显示的颜色
        /// </summary>
        public Color GestureViewMainPathColor
        {
            get { return _gestureView.PathMainColor; }
            set
            {
                if (value == GestureViewMainPathColor) return;

                _gestureView.PathMainColor = value;
                _config.Set(ConfigKeys.GestureViewMainPathColor, value.ToArgb());

                OnPropertyChanged("GestureViewMainPathColor");
            }
        }
        /// <summary>
        /// 获取或设置手势为被识别时轨迹的颜色
        /// </summary>
        public Color GestureViewAlternativePathColor
        {
            get { return _gestureView.PathAlternativeColor; }
            set
            {
                if (value == GestureViewAlternativePathColor) return;

                _gestureView.PathAlternativeColor = value;
                _config.Set(ConfigKeys.GestureViewAlternativePathColor, value.ToArgb());

                OnPropertyChanged("GestureViewAlternativePathColor");
            }
        }

        public Color GestureViewMiddleBtnMainColor
        {
            get { return _gestureView.PathMiddleBtnMainColor; }
            set
            {
                if (value == GestureViewMiddleBtnMainColor) return;

                _gestureView.PathMiddleBtnMainColor = value;
                _config.Set(ConfigKeys.GestureViewMiddleBtnMainColor, value.ToArgb());

                OnPropertyChanged("GestureViewMiddleBtnMainColor");
            }
        }
        #endregion

        #region migrate methods

        internal void ExportTo(string filePath)
        {
            MigrateService.ExportTo(filePath);
        }

        internal void Import(ConfigAndGestures configAndGestures, bool importConfig, bool importGestures, bool mergeGestures)
        {
            //config 必然是合并
            if (importConfig)
            {
                _config.Import(configAndGestures.Config);
                ReApplyConfig();
            }

            if (importGestures)
            {
                _intentStore.Import(configAndGestures.GestureIntentStore, replace: !mergeGestures);
                OnPropertyChanged("IntentStore");
            }

        }

        private void ReApplyConfig()
        {
            AutoStart = _config.Get<bool>(ConfigKeys.AutoStart, AutoStart);
            AutoCheckForUpdate = _config.Get<bool>(ConfigKeys.AutoCheckForUpdate, AutoCheckForUpdate);
            
            //PathTrackerGestureButton = _config.Get<int>(ConfigKeys.PathTrackerGestureButton);
            PathTrackerInitialValidMove = _config.Get<int>(ConfigKeys.PathTrackerInitialValidMove, PathTrackerInitialValidMove);
            PathTrackerInitialStayTimeout = _config.Get<bool>(ConfigKeys.PathTrackerInitialStayTimeout, PathTrackerInitialStayTimeout);
            PathTrackerInitalStayTimeoutMillis = _config.Get<int>(ConfigKeys.PathTrackerInitialStayTimoutMillis, PathTrackerInitalStayTimeoutMillis);
            PathTrackerPreferCursorWindow = _config.Get<bool>(ConfigKeys.PathTrackerPreferCursorWindow, PathTrackerPreferCursorWindow);

            PathTrackerDisableInFullScreen = _config.Get<bool>(ConfigKeys.PathTrackerDisableInFullScreen, PathTrackerDisableInFullScreen);

            PathTrackerStayTimeout = _config.Get<bool>(ConfigKeys.PathTrackerStayTimeout, PathTrackerStayTimeout);
            PathTrackerStayTimeoutMillis = _config.Get<int>(ConfigKeys.PathTrackerStayTimeoutMillis, PathTrackerStayTimeoutMillis);

            GestureViewShowPath = _config.Get<bool>(ConfigKeys.GestureViewShowPath, GestureViewShowPath);
            GestureViewShowCommandName = _config.Get<bool>(ConfigKeys.GestureViewShowCommandName, GestureViewShowCommandName);
            GestureViewFadeOut = _config.Get<bool>(ConfigKeys.GestureViewFadeOut, GestureViewFadeOut);
            GestureViewMainPathColor = Color.FromArgb(_config.Get<int>(ConfigKeys.GestureViewMainPathColor, GestureViewMainPathColor.ToArgb()));
            GestureViewAlternativePathColor = Color.FromArgb(_config.Get<int>(ConfigKeys.GestureViewAlternativePathColor, GestureViewAlternativePathColor.ToArgb()));
            GestureViewMiddleBtnMainColor = Color.FromArgb(_config.Get<int>(ConfigKeys.GestureViewMiddleBtnMainColor, GestureViewMiddleBtnMainColor.ToArgb()));

            GestureParserEnableHotCorners = _config.Get(ConfigKeys.GestureParserEnableHotCorners, _parser.EnableHotCorners);
            GestureParserEnable8DirGesture = _config.Get(ConfigKeys.GestureParserEnable8DirGesture, _parser.Enable8DirGesture);

        }

        #endregion

        #region tab2 props
        /// <summary>
        /// 获取支持的命令类型字典
        /// </summary>
        public Dictionary<string, Type> SupportedCommands { get; private set; }
        /// <summary>
        /// 获取命令视图工厂
        /// </summary>
        public ICommandViewFactory<CommandViewUserControl> CommandViewFactory { get; private set; }
        /// <summary>
        /// 获取内存中的手示意图存储结构
        /// </summary>
        public IGestureIntentStore IntentStore
        {
            get { return _intentStore; }
        }
        /// <summary>
        /// 获取手势解析器
        /// </summary>
        public GestureParser GestureParser { get { return _parser; } }

        #endregion

        #region hotcorners

        public bool GestureParserEnableHotCorners
        {
            get { return _parser.EnableHotCorners; }
            set
            {
                if (value == _parser.EnableHotCorners) return;
                _parser.EnableHotCorners = value;
                Config.Set(ConfigKeys.GestureParserEnableHotCorners, value);
                OnPropertyChanged("GestureParserEnableHotCorners");
            }
        }
        #endregion
        /// <summary>
        /// 以Modal方式呈现主设置窗口
        /// </summary>
        public void ShowDialog()
        {
            _form.ShowDialog();

            _config.Save();
            _intentStore.Save();

            /*using (var proc = Process.GetCurrentProcess())
            {
                Native.SetProcessWorkingSetSize(proc.Handle, -1, -1);
            }*/
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

        }

        public void BringToFront()
        {
            _form.WindowState = FormWindowState.Normal;
            _form.Activate();
            _form.BringToFront();

        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (CommandViewFactory != null)
            {
                CommandViewFactory.Dispose();
            }

            _config = null;
            _parser = null;
            _gestureView = null;
            _intentStore = null;
            _pathTracker = null;

            SupportedCommands.Clear();
            SupportedCommands = null;
            CommandViewFactory = null;

            _form.Dispose();
            _form = null;

            //GC.Collect();
        }


    }
}
