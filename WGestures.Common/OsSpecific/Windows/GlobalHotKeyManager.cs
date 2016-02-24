using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Win32;

namespace WGestures.Common.OsSpecific.Windows
{

    //taken from: http://stackoverflow.com/questions/2450373/set-global-hotkeys-using-c-sharp
    public sealed class GlobalHotKeyManager : IDisposable
    {
        public struct HotKey : IEquatable<HotKey>
        {
            public ModifierKeys modifiers;
            public Keys key;

            public HotKey(ModifierKeys modifiers, Keys key)
            {
                this.modifiers = modifiers;
                this.key = key;
            }

            public override int GetHashCode()
            {
                return modifiers.GetHashCode() ^ key.GetHashCode();
            }

            public bool Equals(HotKey other)
            {
                return modifiers == other.modifiers && key == other.key;
            }

            public override bool Equals(object obj)
            {
                var asThis = obj as HotKey?;

                if (asThis == null) return false;

                return Equals(asThis.Value);

            }

            public byte[] ToBytes()
            {
                return BitConverter.GetBytes( ((ulong)key) | ( ((ulong)modifiers) << 32 ) );
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                if (0 != (modifiers & ModifierKeys.Control)) sb.Append("Ctrl-");
                if (0 != (modifiers & ModifierKeys.Shift)) sb.Append("Shift-");
                if (0 != (modifiers & ModifierKeys.Alt)) sb.Append("Alt-");
                if (0 != (modifiers & ModifierKeys.Win)) sb.Append("Win-");

                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" + ");
                }
                sb.Append(key.ToString());

                return sb.ToString();
            }

            public static HotKey FromBytes(byte[] value)
            {
                var valueUlong = BitConverter.ToUInt64(value, 0);

                var ret = new HotKey();

                ret.modifiers = (ModifierKeys)(valueUlong >> 32); //hiword
                ret.key = (Keys)(valueUlong & 0x00000000FFFFFFFF);//loword

                return ret;
            }
        }


        /// <summary>
        /// Event Args for the event that is fired after the hot key has been pressed.
        /// </summary>
        public class HotKeyEventArgs : EventArgs
        {
            public GlobalHotKeyManager.HotKey HotKey;

            public HotKeyEventArgs(GlobalHotKeyManager.HotKey hk)
            {
                HotKey = hk;
            }
        }

        /// <summary>
        /// The enumeration of possible modifiers.
        /// </summary>
        [Flags]
        public enum ModifierKeys : uint
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                //base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new HotKeyEventArgs(new HotKey(modifier, key)));
                    
                }else
                {
                    base.WndProc(ref m);
                }
            }

            public event EventHandler<HotKeyEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private Dictionary<HotKey, Action<HotKeyEventArgs>>  _keyToAction = new Dictionary<HotKey, Action<HotKeyEventArgs>>();
        private Dictionary<string, HotKey> _idToHotKey = new Dictionary<string, HotKey>();

        public GlobalHotKeyManager()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate (object sender, HotKeyEventArgs args)
            {
                if (HotKeyPreview != null)
                {
                    if(HotKeyPreview(this, GetIdOfHotKey(args.HotKey), args.HotKey))
                    {
                        return;
                    }
                }

                var callback = _keyToAction[args.HotKey];
                if(callback != null) callback(args);
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public bool RegisterHotKey(string id, ModifierKeys modifier, Keys key, Action<HotKeyEventArgs> callback)
        {
            var combined = new HotKey() { modifiers = modifier, key = key };

            return RegisterHotKey(id, combined, callback);
        }

        public bool RegisterHotKey(string id, HotKey hk, Action<HotKeyEventArgs> callback)
        {
            Debug.WriteLine("RegisterHotKey: " + id + " " + hk.ToString());
            if ( _idToHotKey.ContainsKey(id) )
            {
                UnRegisterHotKeyById_internal(id);
            }

            // register the hot key.
            if (User32.RegisterHotKey(_window.Handle, hk.GetHashCode(), (int)hk.modifiers, (int)hk.key))
            {
                _keyToAction.Add(hk, callback);
                _idToHotKey.Add(id, hk);

                if (HotKeyRegistered != null) HotKeyRegistered(id, hk);
                return true;
            }

            return false;
        }

        /*public void UnRegisterHotKey(ModifierKeys modifiers, Keys key)
        {
            UnRegisterHotKey(new HotKey() { modifiers = modifiers, key = key });
        }*/

        private void UnRegisterHotKey(HotKey hk)
        {
            User32.UnregisterHotKey(_window.Handle, hk.GetHashCode());
            _keyToAction.Remove(hk);
        }

        private void UnRegisterHotKeyById_internal(string id, bool publishEvent=false)
        {
            try
            {
                var hk = _idToHotKey[id];
                UnRegisterHotKey(hk);
                _idToHotKey.Remove(id);

                if(HotKeyUnRegistered != null) HotKeyUnRegistered(id, hk);

            }catch(KeyNotFoundException e)
            {
                throw new InvalidOperationException(id + " not registered!", e);
            }

        }

        public void UnRegisterHotKey(string id)
        {
            Debug.WriteLine("Unregister Hotkey by Id: " + id);
            UnRegisterHotKeyById_internal(id, publishEvent: true);
        }

        public HotKey? GetRegisteredHotKeyById(string id)
        {
            HotKey outHk;
            if (_idToHotKey.TryGetValue(id, out outHk))
            {
                return outHk;
            }
            return null;
        }

        public string GetIdOfHotKey(HotKey hk)
        {
            foreach (var k in _idToHotKey.Keys)
            {
                if (_idToHotKey[k].Equals(hk))
                {
                    return k;
                }
            }

            return null;
        }

        /// <summary>
        /// A hot key has been pressed. return 'Handled'.
        /// </summary>
        public event Func<GlobalHotKeyManager, string, HotKey, bool> HotKeyPreview;
        public event Action<string, HotKey> HotKeyRegistered;
        public event Action<string, HotKey> HotKeyUnRegistered;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            foreach (var key in _keyToAction.Keys)
            {
                User32.UnregisterHotKey(_window.Handle, key.GetHashCode());
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }

}
