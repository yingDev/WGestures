using System;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.Core.Commands.Impl
{
    [Named("音量控制")]
    public class ChangeAudioVolumeCommand : AbstractCommand, IGestureModifiersAware
    {
        private InputSimulator _sim = new InputSimulator();

        public override void Execute()
        {
            try
            {
                _sim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE);

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
                _sim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP);
                _sim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN);
            }
            catch (Exception)
            {

                Native.TryResetKeys(new[] { VirtualKeyCode.VOLUME_UP, VirtualKeyCode.VOLUME_DOWN });
            }


            observeModifiers = GestureModifier.Scroll | GestureModifier.MiddleButtonDown;
        }

        public void ModifierTriggered(GestureModifier modifier)
        {
            try
            {
                switch (modifier)
                {
                    case GestureModifier.WheelForward:
                        ReportStatus("+");
                        5.Times(() => _sim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP));
                        break;
                    case GestureModifier.WheelBackward:
                        ReportStatus("-");
                        5.Times(() => _sim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN));
                        break;
                    case GestureModifier.MiddleButtonDown:
                        ReportStatus("x");
                        _sim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE);
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
