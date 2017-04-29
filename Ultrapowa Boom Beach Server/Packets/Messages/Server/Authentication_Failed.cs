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
        internal int RemainingTime;
        internal string Reason;
        internal string RedirectDomain;       
        internal string ResourceFingerprintData;
        internal string UpdateURL;

        public Authentication_Failed(Device client) : base(client)
        {
            this.Identifier = 20103;
            this.UpdateURL = ConfigurationManager.AppSettings["UpdateUrl"];
            this.Version = 2;

            // 7  = Patch
            // 8  = Update Available
            // 10 = Maintenance
            // 11 = Banned
            // 12 = Debug Mode failed?
            // 13 = Acc Locked PopUp
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.ErrorCode);
            this.Data.AddString(this.ResourceFingerprintData); 
            this.Data.AddString(null); 
            this.Data.AddString(this.ContentUrl); 
            this.Data.AddString(this.UpdateURL);
            this.Data.AddString(this.Reason);
            this.Data.AddInt(this.RemainingTime); 
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