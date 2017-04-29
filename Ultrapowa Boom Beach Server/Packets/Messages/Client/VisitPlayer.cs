namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    // Packet 14113
    internal class VisitPlayer : Message
    {
        internal long PlayerID;

        public VisitPlayer(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.PlayerID = Reader.ReadInt64();
        }

        internal override void Process()
        {
            new OutOfSyncMessage(Device).Send();
            //new VisitPlayerData(this.Device).Send();
        }
    }
}