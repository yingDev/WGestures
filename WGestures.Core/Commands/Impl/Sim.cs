using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace WGestures.Core.Commands.Impl
{
    static internal class Sim
    {
        public static InputSimulator Simulator = new InputSimulator();

        public static void KeyDown(VirtualKeyCode k)
        {
            Simulator.Keyboard.KeyDown(k);
        }

        public static void KeyUp(VirtualKeyCode k)
        {
            Simulator.Keyboard.KeyUp(k);
        }

        public static void KeyPress(VirtualKeyCode k)
        {
            Simulator.Keyboard.KeyPress(k);
        }

        public static void TextEntry(string s)
        {
            Simulator.Keyboard.TextEntry(s);
        }
    }
}
