namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    internal class ChangeName : Message
    {
        internal string Name;

        public ChangeName(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
        }

        internal override void Process()
        {
            this.Device.Player.Avatar.AvatarName = this.Name;
            new ChangeNameMessage(Device) { Name = this.Name }.Send();
        }
    }
}