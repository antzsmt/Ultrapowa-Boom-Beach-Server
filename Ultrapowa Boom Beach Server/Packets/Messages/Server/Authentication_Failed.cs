namespace UCS.Packets.Messages.Server
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Core.Crypto;
    using Helpers.List;
    using Logic.Enums;
    using Utilities.Blake2B;
    using Utilities.Sodium;

    // Packet 20103
    internal class Authentication_Failed : Message
    {
        internal string ContentUrl;

        internal int ErrorCode;
        internal string Reason;
        internal string RedirectDomain;
        internal int RemainingTime;
        internal string ResourceFingerprintData;
        internal string UpdateUrl;

        public Authentication_Failed(Device client) : base(client)
        {
            this.Identifier = 20103;
            this.UpdateUrl = ConfigurationManager.AppSettings["UpdateUrl"];
            this.Version = 2;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.ErrorCode);
            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddString(null);
            this.Data.AddString("Clash of Lights Developement");
            this.Data.AddInt(0);
        }

        internal override void Encrypt()
        {
            if (Device.PlayerState >= State.LOGIN)
            {
                var blake = new Blake2BHasher();

                blake.Update(Device.Keys.SNonce);
                blake.Update(Device.Keys.PublicKey);
                blake.Update(Key.PublicKey);

                var Nonce = blake.Finish();
                var encrypted = Device.Keys.RNonce.Concat(Device.Keys.PublicKey).Concat(Data).ToArray();

                Data = new List<byte>(Sodium.Encrypt(encrypted, Nonce, Key.PrivateKey, Device.Keys.PublicKey));
            }

            Length = (ushort)Data.Count;
        }
    }
}