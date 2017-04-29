namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    internal class TopGlobalPlayers : Message
    {
        public TopGlobalPlayers(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {            
        }

        internal override void Process()
        {
            new OutOfSyncMessage(this.Device).Send();
            //new TopGlobalPlayersData(this.Device).Send();
        }
    }
}