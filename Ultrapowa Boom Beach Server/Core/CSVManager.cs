namespace UCS.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json.Linq;
    using Files.CSV;
    using static Core.Logger;
    using Files;

    internal class CSVManager
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();
        internal static Gamefiles Tables;

        public CSVManager()
        {
            try
            { 
                Gamefiles.Add(0, @"Gamefiles/csv/buildings.csv");

                Tables = new Gamefiles();

                foreach (var _File in Gamefiles)
                {
                    Tables.Initialize(new Table(_File.Value), _File.Key);
                }
                Say("CSV Tables  have been succesfully loaded. (" + Gamefiles.Count + ")");
            }
            catch (Exception e)
            {
                Say();
                Error("Error loading Gamefiles. Looks like you have :");
                Error("     -> Edited the Files Wrong");
                Error("     -> Made mistakes by deleting values");
                Error("     -> Entered too High or Low value");
                Error("     -> Please check to these errors");
                Error(null);
                Error(e.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void DownloadLatestCSVFiles()
        {
            try
            {
                var _DownloadString =
                    "http://df70a89d32075567ba62-1e50fe9ed7ef652688e6e5fff773074c.r40.cf1.rackcdn.com/" +
                    ObjectManager.FingerPrint.sha + "/";
                bool DownloadOnlyCSV;
                back:
                Say("Do you want to download only CSV Files or all (Includes SC...)? Y = Yes, N = No.");
                Say("", true);
                var answer = Console.ReadLine().ToUpper();

                if (answer == "Y")
                    DownloadOnlyCSV = true;
                else if (answer == "N")
                    DownloadOnlyCSV = false;
                else
                    goto back;

                var _WC = new WebClient();
                var _FingerPrint = _WC.DownloadString(new Uri(_DownloadString + "fingerprint.json"));

                var jsonObject = JObject.Parse(_FingerPrint);
                var jsonFilesArray = (JArray) jsonObject["files"];

                foreach (JObject _File in jsonFilesArray)
                {
                    var _CSV = _File["file"].ToObject<string>();
                    var _Folder = _CSV.Split('/');

                    if (DownloadOnlyCSV)
                    {
                        if (_Folder[0] == "csv")
                            DownloadFile(_DownloadString, _CSV, _Folder[0], _Folder[1], false, true);
                        else if (_Folder[0] == "logic")
                            DownloadFile(_DownloadString, _CSV, _Folder[0], _Folder[1], false, true);
                    }
                    else
                    {
                        if (_Folder[0] != "sfx")
                            DownloadFile(_DownloadString, _CSV, _Folder[0], _Folder[1]);
                    }
                }
                Say("All Files has been succesfully downloaded!");
            }
            catch (Exception)
            {
            }
        }

        public static void DownloadFile(string _Link, string _Sublink, string _Folder, string _FileName, bool HasSubFolder = false, bool IsCSV = false)
        {
            try
            {
                string _FileLink = _Link + _Sublink;

                if (!Directory.Exists($"Gamefiles/update/{_Folder}"))
                    Directory.CreateDirectory($"Gamefiles/update/{_Folder}");

                if (HasSubFolder)
                {
                    // Todo
                }
                else
                {
                    WebClient _WC = new WebClient();
                    Say($"Downloading '{_FileName}'... ", true);
                    _WC.DownloadFile(new Uri(_FileLink), $"Gamefiles/update/{_Folder}/{_FileName}");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("DONE");
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}