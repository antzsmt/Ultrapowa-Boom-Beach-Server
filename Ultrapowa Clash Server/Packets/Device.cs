namespace UCS.Packets
{
    using System;
    using System.Net.Sockets;
    using Core;
    using Core.Network;
    using Helpers;
    using Helpers.Binary;
    using Logic;
    using Logic.Enums;

    internal class Device
    {
        internal string AdvertiseID;
        internal string AndroidID;

        internal uint ClientSeed;

        internal string Interface;
        internal string IPAddress;
        internal Crypto Keys;
        internal string MACAddress;
        internal string Model;
        internal string OpenUDID;
        internal string OSVersion;
        internal Level Player;


        internal State PlayerState = State.DISCONNECTED;
        internal Socket Socket;

        internal IntPtr SocketHandle;
        internal Token Token;
        internal string VendorID;

        public Device(Socket so)
        {
            Socket = so;
            Keys = new Crypto();
            SocketHandle = so.Handle;
        }

        public Device(Socket so, Token token)
        {
            Socket = so;
            Keys = new Crypto();
            Token = token;
            SocketHandle = so.Handle;
        }

        internal bool Connected => Socket.Connected &&
                                   (!Socket.Poll(1000, SelectMode.SelectRead) || Socket.Available != 0);

        public bool IsClientSocketConnected()
        {
            try
            {
                return !(Socket.Poll(1000, SelectMode.SelectRead) && Socket.Available == 0 || !Socket.Connected);
            }
            catch
            {
                return false;
            }
        }

        internal void Process(byte[] Buffer)
        {
            if (Buffer.Length >= 7)
            {
                var _Header = new int[3];

                using (var Reader = new Reader(Buffer))
                {
                    _Header[0] = Reader.ReadUInt16();
                    Reader.Seek(1);
                    _Header[1] = Reader.ReadUInt16(); 
                    _Header[2] = Reader.ReadUInt16();

                    if (Buffer.Length - 7 >= _Header[1])
                    {
                        if (MessageFactory.Messages.ContainsKey(_Header[0]))
                        {
                            var _Message =
                                Activator.CreateInstance(MessageFactory.Messages[_Header[0]], this, Reader) as Message;

                            _Message.Identifier = (ushort) _Header[0];
                            _Message.Length = (ushort) _Header[1];
                            _Message.Version = (ushort) _Header[2];

                            _Message.Reader = Reader;

                            try
                            {
                                Logger.Write($"Message {_Message.GetType().Name} ({_Header[0]}) is handled");

                                _Message.Decrypt();
                                _Message.Decode();
                                _Message.Process();
                            }
                            catch (Exception Exception)
                            {
                            }
                        }
                        else
                        {
                            Logger.Write($"Message {_Header[0]} is unhandled");
                            Keys.SNonce.Increment();
                        }

                        if (Buffer.Length - 7 - _Header[1] >= 7)
                            Process(Reader.ReadBytes(Buffer.Length - 7 - _Header[1]));
                        else
                            Token.Reset();
                    }
                }
            }
        }
    }
}