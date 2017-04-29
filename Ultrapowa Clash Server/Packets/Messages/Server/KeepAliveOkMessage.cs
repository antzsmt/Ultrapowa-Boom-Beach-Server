using UCS.Packets.Messages.Client;

namespace UCS.Packets.Messages.Server
{
    // Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(Device client, KeepAliveMessage cka) : base(client)
        {
            this.Identifier = 20108;
        }
    }
}