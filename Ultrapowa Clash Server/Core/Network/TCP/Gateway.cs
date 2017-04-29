namespace UCS.Core.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Checker;
    using Core.Settings;
    using Helpers;
    using Packets;

    internal class Gateway
    {
        internal int ConnectedSockets;
        internal Socket Listener;
        internal Mutex Mutex;

        internal SocketAsyncEventArgsPool ReadPool;
        internal SocketAsyncEventArgsPool WritePool;

        internal Gateway()
        {
            ReadPool = new SocketAsyncEventArgsPool();
            WritePool = new SocketAsyncEventArgsPool();

            Initialize();

            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveBufferSize = Constants.ReceiveBuffer,
                SendBufferSize = Constants.SendBuffer,
                Blocking = false,
                NoDelay = true
            };
            Listener.Bind(new IPEndPoint(IPAddress.Any, Utils.ParseConfigInt("ServerPort")));
            Listener.Listen(200);

            Logger.Say();
            Logger.Say(
                $"UBS has been started on {Listener.LocalEndPoint} in {Program._Stopwatch.ElapsedMilliseconds} Milliseconds !");
            Program._Stopwatch.Stop();

            var AcceptEvent = new SocketAsyncEventArgs();
            AcceptEvent.Completed += OnAcceptCompleted;

            StartAccept(AcceptEvent);
        }

        internal void Initialize()
        {
            for (var Index = 0; Index < Constants.MaxOnlinePlayers + 1; Index++)
            {
                var ReadEvent = new SocketAsyncEventArgs();
                ReadEvent.SetBuffer(new byte[Constants.ReceiveBuffer], 0, Constants.ReceiveBuffer);
                ReadEvent.Completed += OnReceiveCompleted;
                ReadPool.Enqueue(ReadEvent);

                var WriterEvent = new SocketAsyncEventArgs();
                WriterEvent.Completed += OnSendCompleted;
                WritePool.Enqueue(WriterEvent);
            }
        }

        internal void StartAccept(SocketAsyncEventArgs AcceptEvent)
        {
            AcceptEvent.AcceptSocket = null;

            if (!Listener.AcceptAsync(AcceptEvent))
                ProcessAccept(AcceptEvent);
        }

        internal void ProcessAccept(SocketAsyncEventArgs AsyncEvent)
        {
            var Socket = AsyncEvent.AcceptSocket;

            if (Socket.Connected && AsyncEvent.SocketError == SocketError.Success)
            {
                Logger.Write($"New client connected -> {((IPEndPoint)Socket.RemoteEndPoint).Address}");

                var ReadEvent = ReadPool.Dequeue();

                if (ReadEvent != null)
                {
                    var device = new Device(Socket)
                    {
                        IPAddress = ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString()
                    };

                    var Token = new Token(ReadEvent, device);
                    Interlocked.Increment(ref ConnectedSockets);
                    ResourcesManager.AddClient(device);

                    Task.Run(() =>
                    {
                        try
                        {
                            if (!Socket.ReceiveAsync(ReadEvent))
                                ProcessReceive(ReadEvent);
                        }
                        catch (Exception)
                        {
                            Disconnect(ReadEvent);
                        }
                    });
                }
            }
            else
            {
                Logger.Write("Not connected or error at ProcessAccept.");
                Socket.Close(5);
            }

            StartAccept(AsyncEvent);
        }

        internal void ProcessReceive(SocketAsyncEventArgs AsyncEvent)
        {
            if (AsyncEvent.BytesTransferred > 0 && AsyncEvent.SocketError == SocketError.Success)
            {
                var Token = AsyncEvent.UserToken as Token;

                Token.SetData();

                try
                {
                    if (Token.Device.Socket.Available == 0)
                    {
                        Token.Process();

                        if (!Token.Aborting)
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                                ProcessReceive(AsyncEvent);
                    }
                    else
                    {
                        if (!Token.Aborting)
                            if (!Token.Device.Socket.ReceiveAsync(AsyncEvent))
                                ProcessReceive(AsyncEvent);
                    }
                }
                catch (Exception)
                {
                    Disconnect(AsyncEvent);
                }
            }
            else
            {
                Disconnect(AsyncEvent);
            }
        }

        internal void OnReceiveCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            ProcessReceive(AsyncEvent);
        }

        internal void Disconnect(SocketAsyncEventArgs AsyncEvent)
        {
            var Token = AsyncEvent.UserToken as Token;
            ResourcesManager.DropClient(Token.Device);
            ReadPool.Enqueue(AsyncEvent);
        }

        internal void OnAcceptCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            ProcessAccept(AsyncEvent);
        }

        internal void Send(Message Message)
        {
            var WriteEvent = WritePool.Dequeue();

            if (WriteEvent != null)
            {
                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                    ProcessSend(Message, WriteEvent);
            }
            else
            {
                WriteEvent = new SocketAsyncEventArgs();

                WriteEvent.SetBuffer(Message.ToBytes, Message.Offset, Message.Length + 7 - Message.Offset);

                WriteEvent.AcceptSocket = Message.Device.Socket;
                WriteEvent.RemoteEndPoint = Message.Device.Socket.RemoteEndPoint;

                if (!Message.Device.Socket.SendAsync(WriteEvent))
                    ProcessSend(Message, WriteEvent);
            }
        }

        internal void ProcessSend(Message Message, SocketAsyncEventArgs Args)
        {
            Message.Offset += Args.BytesTransferred;

            if (Message.Length + 7 > Message.Offset)
                if (Message.Device.Connected)
                {
                    Args.SetBuffer(Message.Offset, Message.Length + 7 - Message.Offset);

                    if (!Message.Device.Socket.SendAsync(Args))
                        ProcessSend(Message, Args);
                }
        }

        internal void OnSendCompleted(object Sender, SocketAsyncEventArgs AsyncEvent)
        {
            WritePool.Enqueue(AsyncEvent);
        }
    }
}