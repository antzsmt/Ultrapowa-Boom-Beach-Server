namespace UCS.Core.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Helpers;

    internal class Settings
    {
        public Settings()
        {
            MaintenanceTimeLeft = Utils.ParseConfigInt("MaintenanceTimeLeft");
            UpdateURL           = Utils.ParseConfigString("UpdateUrl");
            ClientVersion       = Utils.ParseConfigString("ClientVersion");
            UsePatch            = Utils.ParseConfigBoolean("useCustomPatch");
            PatchURL            = Utils.ParseConfigString("PatchURL");

            Logger.Say("Config File has been loaded.");
        }

        internal static int MaintenanceTimeLeft = 0;
        internal static bool UsePatch           = false;
        internal static string UpdateURL        = null;
        internal static string PatchURL         = null;
        internal static string ClientVersion    = null;
    } 
}
