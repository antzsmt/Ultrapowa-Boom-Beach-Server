namespace UCS.Core.Network
{
    using System.Collections.Generic;
    using System.Net.Sockets;
    using Core.Settings;

    internal class SocketAsyncEventArgsPool
    {
        private readonly object Gate = new object();
        internal readonly Stack<SocketAsyncEventArgs> Pool;

        internal SocketAsyncEventArgsPool()
        {
            Pool = new Stack<SocketAsyncEventArgs>(Constants.MaxOnlinePlayers);
        }

        internal SocketAsyncEventArgs Dequeue()
        {
            lock (Gate)
            {
                if (Pool.Count > 0)
                    return Pool.Pop();

                return null;
            }
        }

        internal void Enqueue(SocketAsyncEventArgs Args)
        {
            lock (Gate)
            {
                if (Pool.Count < Constants.MaxOnlinePlayers + 1)
                    Pool.Push(Args);
            }
        }
    }
}