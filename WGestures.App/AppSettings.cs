using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.Common.Product;

namespace WGestures.App
{
    internal static class AppSettings
    {
        public static string CheckForUpdateUrl
        {
            get
            {

#if DEBUG
                return ConfigurationManager.AppSettings.Get(Constants.CheckForUpdateUrlAppSettingKey);// "http://localhost:1226/projects/latestVersion?product=WGestures";

#else
                return ConfigurationManager.AppSettings.Get(Constants.CheckForUpdateUrlAppSettingKey);

#endif
            }
        }

        public static string ProductHomePage
        {
            get { return ConfigurationManager.AppSettings.Get(Constants.ProductHomePageAppSettingKey); }
        }

        public static string UserDataDirectory
        {
            get { return Application.LocalUserAppDataPath; }
        }



        public static string ConfigFilePath
        {
            get { return UserDataDirectory + @"\config.plist"; }
        }

        public static string GesturesFilePath
        {
            get { return UserDataDirectory + @"\gestures.wg2"; }
        }

        public static string DefaultGesturesFilePath
        {
            get { return Application.StartupPath + @"\defaults\gestures.wg2"; }
        }


        public static string ConfigFileVersion
        {
            get { return "1"; }
        }

        public static string GesturesFileVersion
        {
            get { return "3"; }
        }
        
    }
}
