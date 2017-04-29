using System.Collections.Generic;
using System.IO;

namespace UCS.Core.Checker
{
    internal class DirectoryChecker
    {
        public static List<string> badwords = new List<string>();

        public DirectoryChecker()
        {
            Directorys();
            Files();
            LoadFilter();
        }

        public static void LoadFilter()
        {
            if (File.Exists("filter.ucs"))
            {
                var sr = new StreamReader(@"filter.ucs");
                var line = "";
                while ((line = sr.ReadLine()) != null)
                    badwords.Add(line);
            }
        }

        public static void Directorys()
        {
            string[] Dirs =
            {
                "Logs",
                "Patch",
                "Tools",
                "Library",
                "Gamefiles",
                "Gamefiles/update"
            };

            foreach (var Dir in Dirs)
                if (!Directory.Exists(Dir))
                    Directory.CreateDirectory(Dir);
        }

        public static void Files()
        {
        }
    }
}