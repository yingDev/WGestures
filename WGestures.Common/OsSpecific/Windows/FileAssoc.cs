using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

//from http://www.codeproject.com/Tips/1036276/Protocol-Association-Enumerator
namespace WGestures.Common.OsSpecific.Windows.FileAssoc
{
    #region Interop
    #region IAssocHandlers && IEnumAssocHandlers
    [Flags]
    internal enum ASSOC_FILTER
    {
        ASSOC_FILTER_NONE = 0,
        ASSOC_FILTER_RECOMMENDED = 0x1
    };

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("F04061AC-1659-4a3f-A954-775AA57FC083")]
    internal interface IAssocHandler
    {
        int GetName([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
        int GetUIName([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
        int GetIconLocation(
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPath,
            [Out] out int pIndex);
        int IsRecommended();
        int MakeDefault([In, MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
        int Invoke([In, MarshalAs(UnmanagedType.IUnknown)] object pdo);
        int CreateInvoker([In, MarshalAs(UnmanagedType.IUnknown)] object pdo,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppInvoker);
    };

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("973810ae-9599-4b88-9e4d-6ee98c9552da")]
    internal interface IEnumAssocHandlers
    {
        int Next([In, MarshalAs(UnmanagedType.U4)] int celt,
                 [Out, MarshalAs(UnmanagedType.Interface)] out IAssocHandler rgelt,
                 [Out, MarshalAs(UnmanagedType.U4)] out int pceltFetched);
    };
    #endregion

    internal abstract class Win32
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        internal static extern Int32 SHAssocEnumHandlers(
            [In, MarshalAs(UnmanagedType.LPWStr)]
        string pszExtra,
            [In]
        ASSOC_FILTER afFilter,
            [Out, MarshalAs(UnmanagedType.Interface)]
        out IEnumAssocHandlers ppEnumHandler);

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        internal static extern Int32 SHAssocEnumHandlersForProtocolByApplication(
            [In, MarshalAs(UnmanagedType.LPWStr)]
        string protocol,
            ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)]
        out IEnumAssocHandlers ppEnumHandler);

        #region Helpers
        public static bool VerifyResult(int err)
        {
            if (err == 0)
            {
                return true;
            }
            return false;
        }
        public static void VerifyResultWithException(int err)
        {
            if (err != 0)
            {
                throw Marshal.GetExceptionForHR(err);
            }
        }
        #endregion
    }
    #endregion

    public interface IInitializer : IDisposable
    {
        void Initialize(string protocolOrExt);
        void Reset();
    }

    internal interface IInstance
    {
        IEnumAssocHandlers Get { get; }
    }

    public sealed class ExtensionAssociations : IInitializer, IInstance
    {
        #region Fields
        private string _protocolOrExt = string.Empty;
        private IEnumAssocHandlers _enumHandler = null;
        #endregion

        #region Constructor/Destructor
        public ExtensionAssociations()
        {
        }
        ~ExtensionAssociations()
        {
            Dispose();
        }
        #endregion

        #region IAssocEnumInitializer Members
        public void Initialize(string protocolOrExt)
        {
            _protocolOrExt = protocolOrExt;
        }
        public void Reset()
        {
            Dispose();
            Win32.VerifyResultWithException(Win32.SHAssocEnumHandlers
            (_protocolOrExt, ASSOC_FILTER.ASSOC_FILTER_NONE, out _enumHandler));
        }
        #endregion

        #region Internal members
        IEnumAssocHandlers IInstance.Get
        {
            get
            {
                return _enumHandler;
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (_enumHandler != null && Marshal.IsComObject(_enumHandler))
            {
                Marshal.ReleaseComObject(_enumHandler);
            }
            _enumHandler = null;
        }
        #endregion
    }

    public sealed class ProtocolAssociations : IInitializer, IInstance
    {
        #region Fields
        private string _protocolOrExt = string.Empty;
        private static Guid _riid = new Guid("973810ae-9599-4b88-9e4d-6ee98c9552da");
        private IEnumAssocHandlers _enumHandler = null;
        #endregion

        #region Constructor/Destructor
        public ProtocolAssociations()
        {
        }
        ~ProtocolAssociations()
        {
            Dispose();
        }
        #endregion

        #region IAssocEnumInitializer Members
        public void Initialize(string protocolOrExt)
        {
            _protocolOrExt = protocolOrExt;
        }
        public void Reset()
        {
            Dispose();
            Win32.VerifyResultWithException
            (Win32.SHAssocEnumHandlersForProtocolByApplication(_protocolOrExt, ref _riid, out _enumHandler));
        }
        #endregion

        #region Internal members
        IEnumAssocHandlers IInstance.Get
        {
            get
            {
                return _enumHandler;
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (_enumHandler != null && Marshal.IsComObject(_enumHandler))
            {
                Marshal.ReleaseComObject(_enumHandler);
            }
            _enumHandler = null;
        }
        #endregion
    }

    public class Associations<T> : IDisposable, IEnumerable<AssocHandler>,
    IEnumerator<AssocHandler> where T : IInitializer, new()
    {
        #region Fields
        private T _initializer = new T();
        private AssocHandler _current = null;
        #endregion

        #region Constructor/Destructor
        public Associations(string protocolOrExt)
        {
            _initializer.Initialize(protocolOrExt);
            Reset();
        }
        ~Associations()
        {
            Dispose();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _current = null;
            _initializer.Dispose();
        }
        #endregion

        #region IEnumerator<AssocHandler> Members
        public AssocHandler Current
        {
            get
            {
                return _current;
            }
        }
        #endregion

        #region IEnumerator Members
        object IEnumerator.Current
        {
            get
            {
                return _current;
            }
        }
        public bool MoveNext()
        {
            int outCelt = 0;

            IAssocHandler handler = null;
            _current = null;

            try
            {
                ((IInstance)_initializer).Get.Next(1, out handler, out outCelt);
                if (outCelt > 0)
                {
                    _current = new AssocHandler(handler);
                    return true;
                }
            }
            catch { }

            return false;
        }
        public void Reset()
        {
            _initializer.Reset();
        }
        #endregion

        #region IEnumerable<AssocHandler> Members
        public IEnumerator<AssocHandler> GetEnumerator()
        {
            Reset();
            return this;
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            Reset();
            return this;
        }
        #endregion
    }

    public class AssocHandler : IDisposable
    {
        #region Fields
        private IAssocHandler _handler = null;
        #endregion

        #region Constructor/Destructor
        protected AssocHandler()
        {
        }
        internal AssocHandler(IAssocHandler handler)
        {
            _handler = handler;
        }
        ~AssocHandler()
        {
            Dispose();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (_handler != null && Marshal.IsComObject(_handler))
            {
                Marshal.ReleaseComObject(_handler);
            }
            _handler = null;
        }
        #endregion

        #region IconInfo
        public class IconInfo
        {
            public string Path = string.Empty;
            public int Idx = -1;
        }
        #endregion

        #region Public methods
        public string GetName
        {
            get
            {
                string path = string.Empty;
                try { _handler.GetName(out path); }
                catch { }
                return path;
            }
        }
        public string GetUIName
        {
            get
            {
                string name = string.Empty;
                try { _handler.GetUIName(out name); }
                catch { }
                return name;
            }
        }
        public IconInfo GetIconLocation
        {
            get
            {
                IconInfo iconInfo = new IconInfo();
                try { _handler.GetIconLocation(out iconInfo.Path, out iconInfo.Idx); }
                catch { }
                return iconInfo;
            }
        }
        public bool IsRecommended
        {
            get
            {
                bool isRecommended = false;
                try { isRecommended = (_handler.IsRecommended() == 0); }
                catch { }
                return isRecommended;
            }
        }
        public bool MakeDefault(string pszDescription)
        {
            return Win32.VerifyResult(_handler.MakeDefault(pszDescription));
        }
        public void Invoke(object pdo)
        {
            Win32.VerifyResultWithException(_handler.Invoke(pdo));
        }
        public void CreateInvoker(object pdo, out object ppInvoker)
        {
            Win32.VerifyResultWithException(_handler.CreateInvoker(pdo, out ppInvoker));
        }
        #endregion
    }


}
