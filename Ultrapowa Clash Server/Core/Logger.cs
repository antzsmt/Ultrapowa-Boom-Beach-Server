using System;
using System.Configuration;
using System.IO;
using System.Threading;
using static System.Convert;

namespace UCS.Core
{
    internal class Logger
    {
        private static bool ValidLogLevel;
        private static readonly int getlevel = ToInt32(ConfigurationManager.AppSettings["LogLevel"]);

        private static readonly string timestamp = Convert.ToString(DateTime.Today)
            .Remove(10)
            .Replace(".", "-")
            .Replace("/", "-");

        private static readonly string path = "Logs/log_" + timestamp + "_.txt";
        private static readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1);

        public Logger()
        {
            if (getlevel > 2)
            {
                ValidLogLevel = false;
                LogLevelError();
            }
            else
            {
                ValidLogLevel = true;
            }

            if (getlevel != 0 || ValidLogLevel)
            {
                if (!File.Exists($"Logs/log_{timestamp}_.txt"))
                    using (var sw = new StreamWriter($"Logs/log_{timestamp}.txt"))
                    {
                        sw.WriteLineAsync($"Log file created at {DateTime.Now}");
                        sw.WriteLineAsync();
                    }

                if (!Directory.Exists("Logs/Chat_Logs/"))
                    Directory.CreateDirectory("Logs/Chat_Logs/");
            }
        }

        public static async void Write(string text)
        {
            if (getlevel != 0)
                try
                {
                    await _fileLock.WaitAsync();
                    if (getlevel == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("[UBS]    ");
                        Console.ResetColor();
                        Console.WriteLine(text);                        
                    }
                    using (var sw = new StreamWriter(path, true))
                    {
                        await sw.WriteLineAsync("[LOG]    " + text + " at " + DateTime.UtcNow);
                    }
                }
                finally
                {
                    _fileLock.Release();
                }
        }

        public static async void WriteError(string text)
        {
            if (getlevel != 0)
                try
                {
                    await _fileLock.WaitAsync();
                    if (getlevel == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[LOG]    " + text);
                        Console.ResetColor();
                    }
                    using (var sw = new StreamWriter(path, true))
                    {
                        await sw.WriteLineAsync("[LOG]    " + text + " at " + DateTime.UtcNow);
                    }
                }
                finally
                {
                    _fileLock.Release();
                }
        }

        public static void WriteCenter(string _String)
        {
            Console.SetCursorPosition((Console.WindowWidth - _String.Length) / 2, Console.CursorTop);
            Console.WriteLine(_String);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }

        public static void WriteColored(string _String, int ColorID = 1)
        {
            if (ColorID == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(_String);
                Console.ResetColor();
            }
        }

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void Say(string message, bool write = false)
        {
            if (!write)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("[UBS]    ");
                Console.ResetColor();
                Console.WriteLine(message);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("[UBS]    ");
                Console.ResetColor();
                Console.Write(message);
            }
        }

        public static void Say()
        {
            Console.WriteLine();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR]  " + message);
            Console.ResetColor();
        }

        public void LogLevelError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Please choose a valid Log Level");
            Console.WriteLine("UBS Emulator is now closing...");
            Console.ResetColor();
            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}