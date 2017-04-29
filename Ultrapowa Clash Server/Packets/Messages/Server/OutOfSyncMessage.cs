namespace UCS.Packets.Messages.Server
{
    using Helpers.List;


    // Packet 24104
    internal class OutOfSyncMessage : Message
    {
        public OutOfSyncMessage(Device client) : base(client)
        {
            Identifier = 24104;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
        }
    }
}