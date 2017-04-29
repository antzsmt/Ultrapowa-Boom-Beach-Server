namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    // Packet 10108
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Process()
        {
            new KeepAliveOkMessage(this.Device, this).Send();
        }
    }
}