namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Logic.Enums;
    using Packets.Messages.Server;

    // Packet 10100
    internal class SessionRequest : Message
    {
        public int DeviceSo;
        public string Hash;
        public int KeyVersion;
        public int MajorVersion;
        public int MinorVersion;
        public int Protocol;
        public int Store;
        public int Unknown;

        public SessionRequest(Device client, Reader reader) : base(client, reader)
        {
            this.Device.PlayerState = State.SESSION;
        }

        internal override void Decode()
        {
            this.Protocol = Reader.ReadInt32();
            this.KeyVersion = Reader.ReadInt32();
            this.MajorVersion = Reader.ReadInt32();
            this.Unknown = Reader.ReadInt32();
            this.MinorVersion = Reader.ReadInt32();
            this.Hash = Reader.ReadString();
            this.DeviceSo = Reader.ReadInt32();
            this.Store = Reader.ReadInt32();
        }

        internal override void Process()
        {
            new HandshakeSuccess(Device, this).Send();
        }
    }
}