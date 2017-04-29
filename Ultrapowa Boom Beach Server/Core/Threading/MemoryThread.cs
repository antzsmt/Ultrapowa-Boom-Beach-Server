using System;
using System.Linq;
using System.Threading;
using UCS.Core.Settings;
using Timer = System.Timers.Timer;

namespace UCS.Core.Threading
{
    internal class MemoryThread : IDisposable
    {
        private readonly Thread _Thread;
        private Timer _Timer;

        public MemoryThread()
        {
            _Thread = new Thread(() =>
            {
                _Timer = new Timer();
                _Timer.Interval = Constants.CleanInterval;
                _Timer.Elapsed += (s, a) => Clean();
                _Timer.Start();
            });

            _Thread.Priority = ThreadPriority.BelowNormal;

            _Thread.Start();
        }

        public void Dispose()
        {
            _Timer.Stop();
            _Thread.Abort();
        }

        public static void Clean()
        {
            try
            {
                foreach (var _Player in ResourcesManager.m_vInMemoryLevels.Values.ToList())
                    if (!_Player.Client.IsClientSocketConnected())
                    {
                        _Player.Client.Socket.Close();
                        ResourcesManager.DropClient(_Player.Client.SocketHandle);
                    }

                var c = ResourcesManager.m_vOnlinePlayers.Count;
                Console.Title = Program.Title + c;
                Program.OP = c;

                GC.Collect(GC.MaxGeneration);
                GC.WaitForPendingFinalizers();
            }
            catch (Exception)
            {
            }
        }
    }
}