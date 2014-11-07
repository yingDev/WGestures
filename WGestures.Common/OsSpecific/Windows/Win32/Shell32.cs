
//This file contains the common Win32 API of the desktop Windows and the Windows CE/Mobile. 

//Created by Warren Tang on 8/8/2008

using System;
using System.Runtime.InteropServices;
using System.Text;


namespace Win32
{
    public static partial class Shell32
    {
#if PocketPC
        private const string Shell32Dll = "aygshell.dll";
#else
        private const string Shell32Dll = "shell32.dll";
#endif

        #region dll imports

        //http://msdn.microsoft.com/en-us/library/aa456040.aspx
        [DllImport("aygshell.dll")]
        public static extern int SHDeviceLockAndPrompt();

        [DllImport(Shell32Dll)]
        public static extern int SHGetSpecialFolderPath(
            IntPtr hwndOwner, ref string lpszPath, CSIDL nFolder, bool fCreate);

        [DllImport("aygshell")]
        public static extern void SHSendBackToFocusWindow(
             uint uMsg, uint wp, int lp);

        #endregion

        #region enums & structs
        public enum CSIDL
        {
            CSIDL_DESKTOP = 0x0000, //Not supported on Smartphone.
            CSIDL_FAVORITES = 0x0006, //The file system directory that serves as a common repository for the user's favorite items.
            CSIDL_FONTS = 0x0014, //The virtual folder that contains fonts.
            CSIDL_PERSONAL = 0x0005, //The file system directory that serves as a common repository for documents.
            CSIDL_PROGRAM_FILES = 0x0026, //The program files folder.
            CSIDL_PROGRAMS = 0x0002, //The file system directory that contains the user's program groups, which are also file system directories.
            CSIDL_STARTUP = 0x0007, //The file system directory that corresponds to the user's Startup program group. The system starts these programs when a device is powered on.
            CSIDL_WINDOWS = 0x0024  //The Windows folder.
        }
        #endregion

        #region wrappers
        #endregion

        #region SIP

        public enum SIPF : uint
        {
            SIPF_OFF = 0x0000,

            SIPF_ON = 0x0001,

            SIPF_DOCKED = 0x00000002,

            SIPF_LOCKED = 0x00000004
        }

        //public struct RECT
        //{
        //    public int left;
        //    public int top;
        //    public int right;
        //    public int bottom;
        //}

        [DllImport(Shell32Dll)]
        public extern static uint SipGetInfo(SIPINFO pSipInfo);

        [DllImport(Shell32Dll)]
        public extern static uint SipSetInfo(SIPINFO pSipInfo);

        [DllImport(Shell32Dll)]
        public extern static void SipShowIM(SIPF dwFlag);

        public class SIPINFO
        {
            public SIPINFO()
            {
                cbSize = (uint)Marshal.SizeOf(this);
            }

            public uint cbSize;

            public SIPF fdwFlags;

            public GDI32.RECT rcVisibleDesktop;

            public GDI32.RECT rcSipRect;

            public uint dwImDataSize;

            public IntPtr pvImData;
        }

        // SipStatus return values
        public const uint SIP_STATUS_UNAVAILABLE = 0;
        public const uint SIP_STATUS_AVAILABLE = 1;

        [DllImport(Shell32Dll)]
        public extern static uint SipStatus();
        #endregion

        #region
        [DllImport("aygshell.dll")]
        public static extern uint SHFullScreen(IntPtr hwndRequester, SHFS dwState);

        public enum SHFS : uint
        {
            SHFS_SHOWTASKBAR = 0x0001,
            SHFS_HIDETASKBAR = 0x0002,
            SHFS_SHOWSIPBUTTON = 0x0004,
            SHFS_HIDESIPBUTTON = 0x0008,
            SHFS_SHOWSTARTICON = 0x0010,
            SHFS_HIDESTARTICON = 0x0020
        }

        #endregion

        public static string GetSpecialFolderPath(CSIDL id)
        {
            string path = string.Empty;
            SHGetSpecialFolderPath((IntPtr)0, ref path, id, false);
            return path;
        }

        /// <summary>
        /// Show or hide file extentions of the system.(Wrapper)
        /// </summary>
        /// <param name="bShow">True to show, false to hide.</param>

        public static void ShowFileExtension(bool bShow)
        {
            //let error bubble up.
            Shell32.SHELLSTATE state = new Shell32.SHELLSTATE();
            state.fShowExtensions = (uint)(bShow ? 1 : 0);
            Shell32.SHGetSetSettings(ref state, Shell32.SSF.SSF_SHOWEXTENSIONS, true);
        }

        [DllImport("shell32.dll")]
        public extern static void SHGetSetSettings(ref SHELLSTATE lpss, SSF dwMask, bool bSet);

        #region SSF
        [Flags]
        public enum SSF
        {
            SSF_SHOWALLOBJECTS = 0x00000001,
            SSF_SHOWEXTENSIONS = 0x00000002,
            SSF_HIDDENFILEEXTS = 0x00000004,
            SSF_SERVERADMINUI = 0x00000004,
            SSF_SHOWCOMPCOLOR = 0x00000008,
            SSF_SORTCOLUMNS = 0x00000010,
            SSF_SHOWSYSFILES = 0x00000020,
            SSF_DOUBLECLICKINWEBVIEW = 0x00000080,
            SSF_SHOWATTRIBCOL = 0x00000100,
            SSF_DESKTOPHTML = 0x00000200,
            SSF_WIN95CLASSIC = 0x00000400,
            SSF_DONTPRETTYPATH = 0x00000800,
            SSF_SHOWINFOTIP = 0x00002000,
            SSF_MAPNETDRVBUTTON = 0x00001000,
            SSF_NOCONFIRMRECYCLE = 0x00008000,
            SSF_HIDEICONS = 0x00004000,
            SSF_FILTER = 0x00010000,
            SSF_WEBVIEW = 0x00020000,
            SSF_SHOWSUPERHIDDEN = 0x00040000,
            SSF_SEPPROCESS = 0x00080000,
            SSF_NONETCRAWLING = 0x00100000,
            SSF_STARTPANELON = 0x00200000,
            SSF_SHOWSTARTPAGE = 0x00400000
        }
        #endregion

        #region SHELLSTATE
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SHELLSTATE
        {

            /// fShowAllObjects : 1
            ///fShowExtensions : 1
            ///fNoConfirmRecycle : 1
            ///fShowSysFiles : 1
            ///fShowCompColor : 1
            ///fDoubleClickInWebView : 1
            ///fDesktopHTML : 1
            ///fWin95Classic : 1
            ///fDontPrettyPath : 1
            ///fShowAttribCol : 1
            ///fMapNetDrvBtn : 1
            ///fShowInfoTip : 1
            ///fHideIcons : 1
            ///fWebView : 1
            ///fFilter : 1
            ///fShowSuperHidden : 1
            ///fNoNetCrawling : 1
            public uint bitvector1;

            /// DWORD->unsigned int
            public uint dwWin95Unused;

            /// UINT->unsigned int
            public uint uWin95Unused;

            /// LONG->int
            public int lParamSort;

            /// int
            public int iSortDirection;

            /// UINT->unsigned int
            public uint version;

            /// UINT->unsigned int
            public uint uNotUsed;

            /// fSepProcess : 1
            ///fStartPanelOn : 1
            ///fShowStartPage : 1
            ///fSpareFlags : 13
            public uint bitvector2;

            public uint fShowAllObjects
            {
                get
                {
                    return ((uint)((this.bitvector1 & 1u)));
                }
                set
                {
                    this.bitvector1 = ((uint)((value | this.bitvector1)));
                }
            }

            public uint fShowExtensions
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 2u)
                                / 2)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 2)
                                | this.bitvector1)));
                }
            }

            public uint fNoConfirmRecycle
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 4u)
                                / 4)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 4)
                                | this.bitvector1)));
                }
            }

            public uint fShowSysFiles
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 8u)
                                / 8)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 8)
                                | this.bitvector1)));
                }
            }

            public uint fShowCompColor
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 16u)
                                / 16)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 16)
                                | this.bitvector1)));
                }
            }

            public uint fDoubleClickInWebView
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 32u)
                                / 32)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 32)
                                | this.bitvector1)));
                }
            }

            public uint fDesktopHTML
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 64u)
                                / 64)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 64)
                                | this.bitvector1)));
                }
            }

            public uint fWin95Classic
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 128u)
                                / 128)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 128)
                                | this.bitvector1)));
                }
            }

            public uint fDontPrettyPath
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 256u)
                                / 256)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 256)
                                | this.bitvector1)));
                }
            }

            public uint fShowAttribCol
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 512u)
                                / 512)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 512)
                                | this.bitvector1)));
                }
            }

            public uint fMapNetDrvBtn
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 1024u)
                                / 1024)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 1024)
                                | this.bitvector1)));
                }
            }

            public uint fShowInfoTip
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 2048u)
                                / 2048)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 2048)
                                | this.bitvector1)));
                }
            }

            public uint fHideIcons
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 4096u)
                                / 4096)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 4096)
                                | this.bitvector1)));
                }
            }

            public uint fWebView
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 8192u)
                                / 8192)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 8192)
                                | this.bitvector1)));
                }
            }

            public uint fFilter
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 16384u)
                                / 16384)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 16384)
                                | this.bitvector1)));
                }
            }

            public uint fShowSuperHidden
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 32768u)
                                / 32768)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 32768)
                                | this.bitvector1)));
                }
            }

            public uint fNoNetCrawling
            {
                get
                {
                    return ((uint)(((this.bitvector1 & 65536u)
                                / 65536)));
                }
                set
                {
                    this.bitvector1 = ((uint)(((value * 65536)
                                | this.bitvector1)));
                }
            }

            public uint fSepProcess
            {
                get
                {
                    return ((uint)((this.bitvector2 & 1u)));
                }
                set
                {
                    this.bitvector2 = ((uint)((value | this.bitvector2)));
                }
            }

            public uint fStartPanelOn
            {
                get
                {
                    return ((uint)(((this.bitvector2 & 2u)
                                / 2)));
                }
                set
                {
                    this.bitvector2 = ((uint)(((value * 2)
                                | this.bitvector2)));
                }
            }

            public uint fShowStartPage
            {
                get
                {
                    return ((uint)(((this.bitvector2 & 4u)
                                / 4)));
                }
                set
                {
                    this.bitvector2 = ((uint)(((value * 4)
                                | this.bitvector2)));
                }
            }

            public uint fSpareFlags
            {
                get
                {
                    return ((uint)(((this.bitvector2 & 65528u)
                                / 8)));
                }
                set
                {
                    this.bitvector2 = ((uint)(((value * 8)
                                | this.bitvector2)));
                }
            }
        }
        #endregion
    }
}

