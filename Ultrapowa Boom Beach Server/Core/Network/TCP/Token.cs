namespace UCS.Core.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using Core.Settings;
    using Packets;

    internal class Token
    {
        internal bool Aborting;
        internal SocketAsyncEventArgs Args;

        internal byte[] Buffer;
        internal Device Device;

        internal int Offset;
        internal List<byte> Packet;

        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device = Device;
            this.Device.Token = this;

            this.Args = Args;
            this.Args.UserToken = this;

            Buffer = new byte[Constants.ReceiveBuffer];
            Packet = new List<byte>(Constants.ReceiveBuffer);
        }

        internal void SetData()
        {
            byte[] Data = new byte[Args.BytesTransferred];
            Array.Copy(Args.Buffer, 0, Data, 0, Args.BytesTransferred);
            this.Packet.AddRange(Data);
        }

        internal void Process()
        {
            this.Device.Process(this.Packet.ToArray());
        }

        internal void Reset()
        {
            this.Offset = 0;
            this.Packet.Clear();
        }
    }
}