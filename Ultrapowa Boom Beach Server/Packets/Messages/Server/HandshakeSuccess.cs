namespace UCS.Packets.Messages.Server
{
    using Core.Crypto;
    using Helpers.List;
    using Logic.Enums;
    using Packets.Messages.Client;

    // Packet 20100
    internal class HandshakeSuccess : Message
    {
        public HandshakeSuccess(Device client, SessionRequest cka) : base(client)
        {
            this.Identifier = 20100;
            this.Device.PlayerState = State.SESSION_OK;
        }

        internal override void Encode()
        {
            this.Data.AddInt(24);
            this.Data.AddRange(Key.NonceKey);
        }
    }
}