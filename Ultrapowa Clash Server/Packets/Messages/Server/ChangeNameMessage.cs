namespace UCS.Packets.Messages.Server
{
    using Helpers.List;

    internal class ChangeNameMessage : Message
    {
        internal string Name;

        public ChangeNameMessage(Device client) : base(client)
        {
            this.Identifier = 24111;
        }

        internal override void Encode()
        {
            this.Data.AddInt(3);
            this.Data.AddString(Name);
            this.Data.AddHexa("FFFFFF");
        }
    }
}