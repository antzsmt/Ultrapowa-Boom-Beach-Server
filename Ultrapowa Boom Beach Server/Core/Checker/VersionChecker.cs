namespace UCS.Core.Web
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using Newtonsoft.Json.Linq;

    internal class VersionChecker
    {
        public static string GetVersionString()
        {
            try
            {
                var Version = new WebClient().DownloadString(new Uri("https://clashoflights.xyz/UCS/version.json"));
                var obj = JObject.Parse(Version);
                return (string) obj["version"];
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public static string LatestBBVersion()
        {
            try
            {
                return (string)JObject.Parse(new WebClient().DownloadString("http://carreto.pt/tools/android-store-version/?package=com.supercell.boombeach"))["version"];
            }
            catch (Exception)
            {
                return "Couldn't get last BB Version.";
            }
        }
    }
}