
namespace WGestures.App
{
    internal static class Constants
    {
        public const string Identifier = "com.yingdev.WGestures";
        public const string CheckForUpdateUrlAppSettingKey = "CheckForUpdateUrl";

        public const string ProductHomePageAppSettingKey = "ProductHomePage";

#if DEBUG
        public const int AutoCheckForUpdateInterval = 1000 * 3;
#else 
        public const int AutoCheckForUpdateInterval = 1000*60*10;
#endif
    }
}
