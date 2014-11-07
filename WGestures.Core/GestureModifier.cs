using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.Core
{

    [Flags]
    public enum GestureModifier
    {
        None = 0, 
        WheelForward = 1, 
        WheelBackward = 2, 
        MiddleButtonDown = 4, 
        LeftButtonDown = 8,
        RightButtonDown = 16,

        Scroll = WheelBackward | WheelForward,
        All = WheelForward | WheelBackward | MiddleButtonDown | LeftButtonDown | RightButtonDown
    }

    public static class GestureModifierHelper
    {
        public static string ToMnemonic(this GestureModifier modifier)
        {
            switch (modifier)
            {
                case GestureModifier.WheelForward:
                    return "▲";
                case GestureModifier.WheelBackward:
                    return "▼";
                case GestureModifier.MiddleButtonDown:
                    return "●";
                case GestureModifier.LeftButtonDown:
                    return "◐";
                case GestureModifier.RightButtonDown:
                    return "◑";
                default:

                    return String.Empty;
            }
        }
    }
}
