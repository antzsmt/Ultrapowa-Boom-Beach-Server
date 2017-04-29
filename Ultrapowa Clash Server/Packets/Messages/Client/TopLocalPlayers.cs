namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    internal class TopLocalPlayers : Message
    {
        public TopLocalPlayers(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
            new TopGlobalPlayersData(Device).Send();
        }
    }
}