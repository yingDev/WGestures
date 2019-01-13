using System;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.Core.Commands.Impl
{
    [Named("音量控制"),Serializable]
    public class ChangeAudioVolumeCommand : AbstractCommand, IGestureModifiersAware
    {
        public int Delta { get; set; } = 1;

        public override void Execute()
        {
            try
            {
                Sim.KeyPress(VirtualKeyCode.VOLUME_MUTE);

            }
            catch (Exception)
            {
                Native.TryResetKeys(new[] { VirtualKeyCode.VOLUME_MUTE });
            }
        }

        public void GestureRecognized(out GestureModifier observeModifiers)
        {
            try
            {
                Sim.KeyPress(VirtualKeyCode.VOLUME_UP);
                Sim.KeyPress(VirtualKeyCode.VOLUME_DOWN);
            }
            catch (Exception)
            {

                Native.TryResetKeys(new[] { VirtualKeyCode.VOLUME_UP, VirtualKeyCode.VOLUME_DOWN });
            }


            observeModifiers = GestureModifier.Scroll | GestureModifier.MiddleButtonDown;
        }

        public void ModifierTriggered(GestureModifier modifier)
        {
            if (Delta < 1) Delta = 1;
            try
            {
                switch (modifier)
                {
                    case GestureModifier.WheelForward:
                        ReportStatus("+");
                        Delta.Times(() => Sim.KeyPress(VirtualKeyCode.VOLUME_UP));
                        break;
                    case GestureModifier.WheelBackward:
                        ReportStatus("-");
                        Delta.Times(() => Sim.KeyPress(VirtualKeyCode.VOLUME_DOWN));
                        break;
                    case GestureModifier.MiddleButtonDown:
                        ReportStatus("x");
                        Sim.KeyPress(VirtualKeyCode.VOLUME_MUTE);
                        break;

                }
            }
            catch (Exception)
            {
                Native.TryResetKeys(new[] { VirtualKeyCode.VOLUME_UP, VirtualKeyCode.VOLUME_DOWN, VirtualKeyCode.VOLUME_MUTE });
            }

        }

        public void GestureEnded()
        {
            //do nothing

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        public event Action<string> ReportStatus;
    }

    static class IntExtension
    {
        public static void Times(this int n, Action act)
        {
            if (n <= 0) return;

            for (var i = 0; i < n; i++)
            {
                act();
            }
        }
    }
}
