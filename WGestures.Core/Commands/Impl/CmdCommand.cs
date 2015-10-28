using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.Core.Commands.Impl
{
    [Named("命令行"),Serializable, JsonObject(MemberSerialization.OptIn)]
    public class CmdCommand : Commands.AbstractCommand, IGestureContextAware
    {
        private readonly static string explorerPath = Environment.GetEnvironmentVariable("windir").ToLower() + Path.DirectorySeparatorChar + "explorer.exe";

        [JsonProperty]
        public string Code { get; set; }

        [JsonProperty]
        public bool ShowWindow { get; set; }

        [JsonProperty]
        public bool AutoSetWorkingDir { get; set; }

        public GestureContext Context { set; private get; }

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

                        if (exe != null && exe.ToLower() == explorerPath)
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
                if ( Context != null)
                {
                    process.StartInfo.EnvironmentVariables.Add("WG_PROCID", Context.ProcId.ToString());
                    process.StartInfo.EnvironmentVariables.Add("WG_WINID", Context.WinId.ToString());
                    process.StartInfo.EnvironmentVariables.Add("WG_STARTPOINT_X", Context.StartPoint.X.ToString());
                    process.StartInfo.EnvironmentVariables.Add("WG_STARTPOINT_Y", Context.StartPoint.Y.ToString());
                    process.StartInfo.EnvironmentVariables.Add("WG_ENDPOINT_X", Context.EndPoint.X.ToString());
                    process.StartInfo.EnvironmentVariables.Add("WG_ENDPOINT_Y", Context.EndPoint.X.ToString());
                }

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WindowStyle = ShowWindow ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = !ShowWindow;
                process.Start();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }

        }

        //获得活动的explore窗口的路径
        private static string GetExplorerCurrentPath()
        {
            string path = null;
            
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
