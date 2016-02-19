using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.Core
{
    public enum GestureButtons : short
    {
        MiddleButton = 1, RightButton = 0,
        LeftButton = 2, XButton = 3
    }

    public static class GestureButtonsHelper
    {
        public static string ToMnemonic(this GestureButtons gestureBtn)
        {
            switch (gestureBtn)
            {
                case GestureButtons.MiddleButton:
                    return "●";
                case GestureButtons.RightButton:
                    return string.Empty;
                case GestureButtons.LeftButton:
                    return "◐";
                default:
                    return string.Empty;
            }
        }
    }
}
