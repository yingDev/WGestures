using System;
using System.Diagnostics;
using System.IO;
using WGestures.Common.Annotation;

namespace WGestures.Core.Commands.Impl
{
    [Named("打开文件或应用程序"), Serializable]
    public class OpenFileCommand : Commands.AbstractCommand
    {
        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
            }
        }

        public OpenFileCommand()
        {
            FilePath = "";
        }

        public override void Execute()
        {
            var info = new ProcessStartInfo(FilePath);
            info.UseShellExecute = true;

            var p = Process.Start(info);
            if(p != null) p.Close();
        }

        public override string Description()
        {
            if (File.Exists(FilePath))
            {
                return "打开 "+Path.GetFileName(FilePath);
            }

            return "";
        }
    }
}