using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace WGestures.Common.Config.Impl
{
    public class PlistConfig : AbstractDictConfig
    {
        public string FileVersion
        {
            get { return Get<string>("$$FileVersion", null); }
            set
            {
                Set("$$FileVersion",value);
            }
        }

        public string PlistPath { get; set; }

        /// <summary>
        /// 创建一个空的Config
        /// </summary>
        public PlistConfig(){}

        /// <summary>
        /// 创建并指定要加载或保存的plist文件位置
        /// </summary>
        /// <param name="plistPath">Plist path.</param>
        public PlistConfig(string plistPath)
        {
            PlistPath = plistPath;
            Load();
        }

        public PlistConfig(Stream stream, bool closeStream)
        {
            Load(stream,closeStream);
        }

        private void Load()
        {
            if (PlistPath == null)
                throw new InvalidOperationException("未指定需要加载的plist文件路径");
            if (!File.Exists(PlistPath))
            {
                return;
            }
            Dict = (Dictionary<string, object>)Plist.readPlist(PlistPath);
        }

        private void Load(Stream stream, bool closeStream = false)
        {
            if(stream == null || !stream.CanRead) throw new ArgumentException("stream");
 
            try
            {
                Dict = (Dictionary<string, object>)Plist.readPlist(stream, plistType.Auto);
            }
            catch (Exception)
            {
                if (closeStream && stream != null) stream.Close();
                throw;
            }
                

            

        }

        public override void Save()
        {
            if (PlistPath == null)
                throw new InvalidOperationException("未指定需要保存到的plist文件路径(PlistPath属性)");
            Plist.writeXml(Dict, PlistPath);
        }

    }

}
