using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.Core
{
    [Flags]
    public enum GestureTriggerButton
    {
        None =0, Right = 1, Middle = 2, X1 = 4, X2 = 8,
        X = X1 | X2
    }

    public static class GestureTriggerButtonExtension
    {
        public static string ToMnemonic(this GestureTriggerButton gestureBtn)
        {
            switch (gestureBtn)
            {
                case GestureTriggerButton.Middle:
                    return "●";
                case GestureTriggerButton.Right:
                    return "◑";
                case GestureTriggerButton.X1:
                    return "X1";
                case GestureTriggerButton.X2:
                    return "X2";

                default:
                    return string.Empty;
            }
        }
    }
}
