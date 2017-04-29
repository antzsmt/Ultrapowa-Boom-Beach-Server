namespace UCS.Packets.Messages.Client
{
    using System.Linq;
    using Core;
    using Core.Crypto;
    using Core.Network;
    using Helpers.Binary;
    using Logic;
    using Logic.Enums;
    using Packets.Messages.Server;
    using Utilities.Blake2B;
    using Utilities.Sodium;
    using System.Configuration;
    using System;
    using static Core.Settings.Settings;

    // Packet 10101
    internal class Authentication : Message
    {
        public string AdvertisingGUID;
        public bool Android;
        public string AndroidDeviceID;
        public int ContentVersion;
        public string DeviceModel;
        public string FacebookDistributionID;
        public bool IsAdvertisingTrackingEnabled;
        public Level Level;
        public int LocaleKey;
        public string MacAddress;
        public int MajorVersion;
        public string MasterHash;
        public int MinorVersion;
        public string OpenUDID;
        public string OSVersion;
        public string Region;
        public uint Seed;
        public string UDID;
        public long UserID;
        public string UserToken;
        public string VendorGUID;

        public Authentication(Device device, Reader reader) : base(device, reader)
        {
            this.Device.PlayerState = State.LOGIN;
        }

        internal override void Decrypt()
        {
            var Buffer = Reader.ReadBytes(Length);
            Device.Keys.PublicKey = Buffer.Take(32).ToArray();

            var Blake = new Blake2BHasher();

            Blake.Update(Device.Keys.PublicKey);
            Blake.Update(Key.PublicKey);

            Device.Keys.RNonce = Blake.Finish();

            Buffer = Sodium.Decrypt(Buffer.Skip(32).ToArray(), Device.Keys.RNonce, Key.PrivateKey,
                Device.Keys.PublicKey);
            Device.Keys.SNonce = Buffer.Skip(24).Take(24).ToArray();
            Reader = new Reader(Buffer.Skip(48).ToArray());

            Length = (ushort) Buffer.Length;
        }

        internal override void Decode()
        {
            this.UserID = Reader.ReadInt64();
            this.UserToken = Reader.ReadString();
            this.MajorVersion = Reader.ReadInt32();
            this.ContentVersion = Reader.ReadInt32();
            this.MinorVersion = Reader.ReadInt32();
            this.MasterHash = Reader.ReadString();
            this.UDID = Reader.ReadString();
            this.OpenUDID = Reader.ReadString();
            this.DeviceModel = Reader.ReadString();
            this.Reader.ReadString();
            this.Region = Reader.ReadString();
            this.AdvertisingGUID = Reader.ReadString();
            this.OSVersion = Reader.ReadString();
            this.Reader.ReadString();
        }

        internal override void Process()
        {
            if (MaintenanceTimeLeft != 0)
            {
                new Authentication_Failed(this.Device)
                {
                    ErrorCode = 10,
                    RemainingTime = MaintenanceTimeLeft
                }.Send();
                return;
            }

            if (ClientVersion != $"{this.MajorVersion}.{this.MinorVersion}")
            {
                new Authentication_Failed(this.Device)
                {
                    ErrorCode = 8,
                    UpdateURL = UpdateURL
                }.Send();
                return;
            }

            this.CheckClient();
        }

        private void LogUser()
        {
            if (Level != null)
            {
                ResourcesManager.LogPlayerIn(Level, this.Device);
                Level.Tick();
                Level.Avatar.IPAddress = this.Device.IPAddress;

                new Authentication_OK(this.Device)
                {
                    ServerMajorVersion = this.MajorVersion,
                    ServerBuild = this.MinorVersion,
                    ContentVersion = this.ContentVersion
                }.Send();

                new AvatarStreamMessage(this.Device).Send();
                new OwnHomeDataMessage(this.Device).Send();
            }
            else
            {
                new Authentication_Failed(this.Device) { ErrorCode = 1 }.Send(); // TODO Reason...
            }
        }

        private void CheckClient()
        {
            if (this.UserID <= 0 || this.UserToken == string.Empty)
            {
                this.NewUser();
            }
            else
            {
                Level = ResourcesManager.GetPlayer(this.UserID);
                this.LogUser();
            }
        }

        private void NewUser()
        {
            Level = ObjectManager.CreateAvatar;

            if (string.IsNullOrEmpty(UserToken))
                for (var i = 0; i < 20; i++)
                {
                    var Letter = (char) Resources.Random.Next('A', 'Z');
                    Level.Avatar.UserToken += Letter;
                }

            //Level.Avatar.Region = this.Region.ToUpper();
            Level.Avatar.InitializeAccountCreationDate();
            //Level.Avatar.m_vAndroid = this.Android;           

            Resources.DatabaseManager.Save(Level);
            this.LogUser();
        }
    }
}