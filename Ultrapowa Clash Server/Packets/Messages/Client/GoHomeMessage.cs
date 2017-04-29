namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    // Packet 14101
    internal class GoHomeMessage : Message
    {
        public GoHomeMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {

        }

        internal override async void Process()
        {
            new OwnHomeDataMessage(this.Device).Send();
        }
    }
}