using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.Core.Commands.Impl
{
    [Named("命令行"),Serializable]
    public class CmdCommand : Commands.AbstractCommand
    {
        private readonly static string explorerPath = Environment.GetEnvironmentVariable("windir") + Path.DirectorySeparatorChar + "explorer.exe";


        public string Code { get; set; }

        public bool ShowWindow { get; set; }

        public bool AutoSetWorkingDir { get; set; }

        public CmdCommand()
        {
            ShowWindow = true;
            AutoSetWorkingDir = true;
        }

        public override void Execute()
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = (ShowWindow ? "/k " : "/C ") + string.Join(" & ",Code.Split(new[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries));

                var workingDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                //设置工作目录,如果当前应用程序时explorer，则为其当前位置
                if (AutoSetWorkingDir)
                {
                    try
                    {
                        var exe = Native.GetProcessFile(Native.GetActiveProcessId());

                        if (exe == explorerPath)
                        {
                            workingDir = GetExplorerCurrentPath() ?? workingDir;

                        }else if (exe != null)
                        {
                            workingDir = Path.GetDirectoryName(exe);
                        }

                    }
                    catch (Exception)
                    { //ignore
                    }
                }

                process.StartInfo.WorkingDirectory = workingDir;

                process.StartInfo.WindowStyle = ShowWindow ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
                process.Start();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }

        }

        //获得活动的explore窗口的路径
        private static string GetExplorerCurrentPath()
        {
            string path = null;

            //TODO: ReleaseCom
            var shellWindows = new SHDocVw.ShellWindows();

            try
            {

                foreach (SHDocVw.InternetExplorer ie in shellWindows)
                {
                    var filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

                    var activeWindow = Native.GetForegroundWindow();

                    if (filename.Equals("explorer") && activeWindow == new IntPtr(ie.HWND))
                    {
                        var uri = ie.LocationURL;
                        if (Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                        {
                            path = new Uri(uri).LocalPath;
                        }
                    }
                }

                return path;
            }
            finally
            {
                Marshal.ReleaseComObject(shellWindows);
            }

        }
    }
}
