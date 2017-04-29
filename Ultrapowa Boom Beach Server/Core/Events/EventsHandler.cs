using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UCS.Logic.Enums;

namespace UCS.Core.Events
{
    internal class EventsHandler
    {
        internal static EventHandler EHandler;

        internal EventsHandler()
        {
            EHandler += Handler;
            SetConsoleCtrlHandler(EHandler, true);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        internal void ExitHandler()
        {
            try
            {
                if (ResourcesManager.m_vInMemoryLevels.Count > 0)
                    Parallel.ForEach(ResourcesManager.m_vInMemoryLevels.Values.ToList(), _Player =>
                    {
                        if (_Player != null)
                            ResourcesManager.LogPlayerOut(_Player);
                    });


                /*if (ResourcesManager.m_vInMemoryAlliances.Count > 0)
                {
                    Parallel.ForEach(ResourcesManager.m_vInMemoryAlliances.Values, (_Player) =>
                    {
                        if (_Player != null)
                        {
                            ResourcesManager.RemoveAllianceFromMemory(_Player.m_vAllianceId);
                        }
                    });
                }*/
            }
            catch (Exception)
            {
                Logger.Write("Failed to save all Players/Clans.");
            }
        }

        internal void Handler(Exits Type = Exits.CTRL_CLOSE_EVENT)
        {
            Logger.Say("UBS is shutting down", true);
            new Thread(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }
            }).Start();
            ExitHandler();
        }

        internal delegate void EventHandler(Exits Type = Exits.CTRL_CLOSE_EVENT);
    }
}