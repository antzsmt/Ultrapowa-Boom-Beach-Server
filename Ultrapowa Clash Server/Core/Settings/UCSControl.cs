using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static UCS.Core.Logger;

namespace UCS.Core.Settings
{
    internal class UCSControl
    {
        public static void UCSClose()
        {
            Say("UBS is shutting down", true);
            new Thread(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }
            }).Start();

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
            }

            Environment.Exit(0);
        }

        public static void UCSRestart()
        {
            new Thread(() =>
            {
                Say("Restarting UBS...");
                Thread.Sleep(200);
                Process.Start("UBS.exe");
                Environment.Exit(0);
            }).Start();
        }
    }
}