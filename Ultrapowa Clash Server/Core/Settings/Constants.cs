namespace UCS.Core.Settings
{
    using System.Reflection;
    using Helpers;

    internal class Constants
    {
        public const int CleanInterval   = 6000;

        internal const int SendBuffer    = 2048;
        internal const int ReceiveBuffer = 2048;
        public static string Version     = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string Build       = "23";

        public static readonly bool UseCacheServer = Utils.ParseConfigBoolean("CacheServer");
        public static int MaxOnlinePlayers         = Utils.ParseConfigInt("MaxOnlinePlayers");
    }
}