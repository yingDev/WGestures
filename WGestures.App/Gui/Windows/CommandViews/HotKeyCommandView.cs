using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class HotKeyCommandView : CommandViewUserControl
    {

        private static readonly VirtualKeyCode[] modifierKeys = { VirtualKeyCode.CONTROL, VirtualKeyCode.LCONTROL,VirtualKeyCode.RCONTROL, 
                                                                    VirtualKeyCode.MENU, VirtualKeyCode.LMENU,VirtualKeyCode.RMENU, 
                                                                    VirtualKeyCode.SHIFT, VirtualKeyCode.LSHIFT, VirtualKeyCode.RSHIFT, 
                                                                    VirtualKeyCode.RWIN, VirtualKeyCode.LWIN};


        private List<VirtualKeyCode> _keys = new List<VirtualKeyCode>();
        private List<VirtualKeyCode> _modifiers = new List<VirtualKeyCode>();
        private HashSet<VirtualKeyCode> _pressedKeys = new HashSet<VirtualKeyCode>(); 

        private GlobalKeyboardHook hook = new GlobalKeyboardHook();

        //避免委托被回收
        private KeyEventHandler _keyboardHookOnKeyDown;
        private KeyEventHandler _keyboardhookOnKeyUp;


        public HotKeyCommandView()
        {
            InitializeComponent();


            _keyboardHookOnKeyDown = KeyboardHookOnKeyDown;
            _keyboardhookOnKeyUp = KeyboardHookOnKeyUp;

            hook.KeyDown += _keyboardHookOnKeyDown;
            hook.KeyUp += _keyboardhookOnKeyUp;


        }

        private int _lastKey = -1;
        private void KeyboardHookOnKeyDown(object sender, KeyEventArgs args)
        {
            args.Handled = true;

            if (!Enum.IsDefined(typeof (VirtualKeyCode), args.KeyValue)) return;
            
            var key = (VirtualKeyCode)args.KeyValue;
            if(!_pressedKeys.Add(key)) return;
                
            if (modifierKeys.Contains(key) && !_modifiers.Contains(key))
            {
                _modifiers.Add(key);
            }
            else if(!_keys.Contains(key))
            {
                _keys.Add(key);
            }
        }

        private void KeyboardHookOnKeyUp(object sender, KeyEventArgs args)
        {
            args.Handled = true;
            if(args.KeyValue == _lastKey) _lastKey = -1;

            if (Enum.IsDefined(typeof(VirtualKeyCode), args.KeyValue))
            {
                var key = (VirtualKeyCode)args.KeyValue;

                if (_modifiers.Contains(key) || _keys.Contains(key))
                {
                    _pressedKeys.Remove(key);
                }
            }

            if (_pressedKeys.Count == 0)
            {
                btn_recordHotkey.PerformClick();
            }

        }

        private bool _isRecording = false;
        private HotKeyCommand _command;

        public override AbstractCommand Command
        {
            get { return _command; }
            set
            {
                _command = (HotKeyCommand)value;

                _keys = _command.Keys;
                _modifiers = _command.Modifiers;

                lb_shortcut.Text = HotKeyCommand.HotKeyToString(_command.Modifiers, _command.Keys);

            }
        }

        private void btn_recordHotkey_Click(object sender, EventArgs e)
        {
            if (!_isRecording)
            {
                StartRecord();
            }
            else
            {
                EndRecord();
            }
        }

        private void StartRecord()
        {
            _isRecording = true;
            _lastKey = -1;
            _pressedKeys.Clear();

            _modifiers.Clear();
            _keys.Clear();

            lb_shortcut.Text = "...";

            btn_recordHotkey.BackColor = Color.OrangeRed;
            btn_recordHotkey.ForeColor = Color.White;
            btn_recordHotkey.Text = "请按快捷键";

            hook.hook();
        }

        private void EndRecord()
        {
            hook.unhook();

            btn_recordHotkey.BackColor = SystemColors.Control;
            btn_recordHotkey.ForeColor = Color.Black;
            btn_recordHotkey.Text = "录入快捷键";

            lb_shortcut.Text = HotKeyCommand.HotKeyToString(_modifiers, _keys);

            _command.Keys = _keys;
            _command.Modifiers = _modifiers;
            _isRecording = false;

            OnCommandValueChanged();


        }


    }
}
