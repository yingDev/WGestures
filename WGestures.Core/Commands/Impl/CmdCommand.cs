using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using System.Linq;
using System.Text.RegularExpressions;

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

        public override string Description()
        {
            if(Code != null)
            {
                var lines = Code.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if(lines.Length > 0)
                {
                    var regex = new Regex(@"^(::|rem )(.*)");
                    var matches = regex.Matches(lines[0].Trim());
                    if(matches.Count > 0)
                    {
                        var msg = matches[0].Groups[2].Value.Trim();
                        if (msg.Length > 8) msg = msg.Substring(0, 8);
                        if (msg.Length > 0)
                        {
                            return msg;
                        }
                    }
                    
                }

            }

            return base.Description();
        }

        public override void Execute()
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = (ShowWindow ? "/k " : "/C ") + NormalizedCode;
                //string.Join(" & ",Code.Split(new[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries));

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

        private string NormalizedCode
        {
            get
            {
                var sb = new StringBuilder(Code.Length);
                var lines = Code.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var l in lines)
                {

                    var trimmed = l.Trim();
                    
                    if(trimmed.StartsWith("::") || trimmed.StartsWith("rem ", StringComparison.OrdinalIgnoreCase)
                        || trimmed == "rem")
                    {
                        continue;
                    }

                    var indexOfRem = l.LastIndexOf("::");
                    if(indexOfRem < 0)
                    {
                        indexOfRem = l.LastIndexOf(" rem ", StringComparison.OrdinalIgnoreCase);
                    }
                    if(indexOfRem > 0)
                    {
                        trimmed = l.Substring(0, indexOfRem);
                    }

                    sb.Append(trimmed);
                    if(l != lines[lines.Length -1])
                    {
                        sb.Append(" & ");
                    }
                }

                return sb.ToString();
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
