namespace UCS.Packets.Messages.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    internal class RequestAvatarStream : Message
    {
        internal long PlayerID;

        public RequestAvatarStream(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
            new AvatarStreamMessage(this.Device).Send();
        }
    }
}