namespace UCS
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Core;
    using Core.Checker;
    using Core.Network;
    using Core.Settings;
    using Core.Threading;
    using Core.Web;
    using Helpers;
    using WebAPI;
    using static Core.Logger;

    internal class Program
    {
        internal static int OP                   = 0;
        internal static string Title             = $"Ultrapowa Boom Beach Server v{Constants.Version} - ©Ultrapowa | Online Players: ";
        public static Stopwatch _Stopwatch       = new Stopwatch();
        public static string Version             = string.Empty;

        internal static void Main()
        {
            IntPtr Handle = GetConsoleWindow();
            SetWindowLong(Handle, -20, (int)GetWindowLong(Handle, -20) ^ 0x80000);

            if (Utils.ParseConfigBoolean("Animation"))
            {

                new Thread(() =>
                {
                    for (int i = 20; i < 227; i++)
                    {
                        if (i < 100)
                        {
                            SetLayeredWindowAttributes(Handle, 0, (byte)i, (uint)0x2);
                            Thread.Sleep(5);
                        }
                        else
                        {
                            SetLayeredWindowAttributes(Handle, 0, (byte)i, (uint)0x2);
                            Thread.Sleep(15);
                        }
                    }
                }).Start();
            }
            else
            {
                SetLayeredWindowAttributes(Handle, 0, 227, (uint)0x2);
            }

            Console.Title = Title + OP;

            Say();

            Console.ForegroundColor = ConsoleColor.Green;
            Logger.WriteCenter(@" ____ ___.__   __                                                  ");
            Logger.WriteCenter(@"|    |   \  |_/  |_____________  ______   ______  _  _______       ");
            Logger.WriteCenter(@"|    |   /  |\   __\_  __ \__  \ \____ \ /  _ \ \/ \/ /\__  \      ");
            Logger.WriteCenter(@"|    |  /|  |_|  |  |  | \// __ \|  |_> >  <_> )     /  / __ \_    ");
            Logger.WriteCenter(@"|______/ |____/__|  |__|  (____  /   __/ \____/ \/\_/  (____  /    ");
            Logger.WriteCenter(@"                               \/|__|                       \/     ");
            Logger.WriteCenter("            ");

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Logger.WriteCenter("+-------------------------------------------------------+");
            Console.ResetColor();
            Logger.WriteCenter("|This program is made by the Ultrapowa Development Team.|");
            Logger.WriteCenter("|    Ultrapowa is not affiliated to \"Supercell, Oy\".    |");
            Logger.WriteCenter("|        This is licensed under the MIT License.        |");
            Logger.WriteCenter("|   Visit www.ultrapowa.com daily for News & Updates!   |");
            Console.ForegroundColor = ConsoleColor.Green;
            Logger.WriteCenter("+-------------------------------------------------------+");
            Console.ResetColor();

            Say();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[UBS]    ");
            //Version = VersionChecker.GetVersionString();

            _Stopwatch.Start();

            if (/*Version == Constants.Version*/ true)
            {
                Console.WriteLine($"> UBS is up-to-date: {Constants.Version}");
                Console.ResetColor();
                Say();
                Say("Preparing Server...\n");

                Resources.Initialize();
            }
            else if (Version == "Error")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("> An Error occured when requesting the Version.");
                Console.WriteLine();
                Logger.Say("Please contact the Support at https://ultrapowa.com/forum!");
                Console.WriteLine();
                Logger.Say("Aborting...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> UBS is not up-to-date! New Version: {Version}. Aborting...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static void UpdateTitle() { Console.Title = Title + OP; }

        public static void TitleU() { Console.Title = Title + ++OP; }

        public static void TitleD() { Console.Title = Title + --OP; }

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();
    }
}
