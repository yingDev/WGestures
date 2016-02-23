using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.App
{
    public static  class ConfigKeys
    {
        public const string AutoStart = "AutoStart";
        public const string AutoCheckForUpdate = "AutoCheckForUpdate";

        public const string PathTrackerTriggerButton = "PathTrackerTriggerButton";
        public const string PathTrackerInitialValidMove = "PathTrackerInitialValidMove";
        public const string PathTrackerStayTimeout = "PathTrackerStayTimeout";
        public const string PathTrackerStayTimeoutMillis = "PathTrackerStayTimeoutMillis";
        public const string PathTrackerInitialStayTimeout = "PathTrackerInitialStayTimeout";
        public const string PathTrackerInitialStayTimoutMillis = "PathTrackerInitialStayTimoutMillis";
        public const string PathTrackerPreferCursorWindow = "PathTrackerPreferCursorWindow";

        //历史原因， 字符串不变
        public const string PathTrackerDisableInFullScreen = "GestureParserDisableInFullScreenMode";

        public const string GestureViewShowPath = "GestureViewShowPath";
        public const string GestureViewShowCommandName = "GestureViewShowCommandName";
        public const string GestureViewFadeOut = "GestureViewFadeOut";
        public const string GestureViewMainPathColor = "GestureViewMainPathColor";
        public const string GestureViewMiddleBtnMainColor = "GestureViewMiddleBtnMainColor";
        public const string GestureViewAlternativePathColor = "GestureViewAlternativePathColor";
        public const string GestureViewXBtnPathColor = "GestureViewXBtnPathColor";


        public const string IsFirstRun = "IsFirstRun";
        public const string Is360EverDected = "Is360EverDected";

        public const string TrayIconVisible = "TrayIconVisible";

        public const string GestureParserEnableHotCorners = "GestureParserEnableHotCorners";
        public const string GestureParserEnable8DirGesture = "GestureParserEnable8DirGesture";

        public const string GestureParserEnableRubEdges = "GestureParserEnableRubEdges";

        public const string PauseResumeHotKey = "PauseResumeHotKey";
        public const string EnableWindowsKeyGesturing = "EnableWindowsKeyGesturing";
    }
}
