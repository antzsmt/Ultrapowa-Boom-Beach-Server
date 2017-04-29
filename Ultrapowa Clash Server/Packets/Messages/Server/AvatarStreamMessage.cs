namespace UCS.Packets.Messages.Server
{
    using Helpers.List;

    // Packets 24411
    internal class AvatarStreamMessage : Message
    {
        public AvatarStreamMessage(Device client) : base(client)
        {
            Identifier = 24411;
        }

        internal override void Encode()
        {
            this.Data.AddHexa("0000000000000000");
        }
    }
}