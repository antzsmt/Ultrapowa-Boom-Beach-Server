namespace UCS.Packets.Messages.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Crypto;
    using Helpers.List;
    using Logic.Enums;
    using Utilities.Blake2B;
    using Utilities.Sodium;

    // Packet 20104
    internal class Authentication_OK : Message
    {
        internal int ContentVersion;

        internal int ServerBuild;
        internal int ServerMajorVersion;

        public Authentication_OK(Device client) : base(client)
        {
            this.Identifier = 20104;
            this.Version = 1;
            this.Device.PlayerState = State.LOGGED;           
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.Device.Player.Avatar.UserID);
            this.Data.AddLong(this.Device.Player.Avatar.UserID);

            this.Data.AddString(this.Device.Player.Avatar.UserToken);
            this.Data.AddString(string.Empty);
            this.Data.AddString(null);

            this.Data.AddInt(this.ServerMajorVersion);
            this.Data.AddInt(this.ServerBuild);
            this.Data.AddInt(this.ContentVersion);

            this.Data.AddString("prod");

            this.Data.AddString(string.Empty);
            this.Data.AddString("DE");
            this.Data.AddString(string.Empty);
            this.Data.AddString(string.Empty);

            this.Data.AddByte(0);
            this.Data.AddString(string.Empty);
            this.Data.AddInt(0);
        }

        internal override void Encrypt()
        {
            var blake = new Blake2BHasher();

            blake.Update(Device.Keys.SNonce);
            blake.Update(Device.Keys.PublicKey);
            blake.Update(Key.PublicKey);

            var Nonce = blake.Finish();
            var encrypted = Device.Keys.RNonce.Concat(Device.Keys.PublicKey).Concat(Data).ToArray();

            Data = new List<byte>(Sodium.Encrypt(encrypted, Nonce, Key.PrivateKey, Device.Keys.PublicKey));
            Length = (ushort) Data.Count;
        }
    }
}