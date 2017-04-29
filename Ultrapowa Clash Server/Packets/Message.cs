namespace UCS.Packets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Core.Settings;
    using Helpers;
    using Helpers.Binary;
    using Helpers.List;
    using Logic.Enums;
    using Utilities.Sodium;

    internal class Message
    {
        internal List<byte> Data;

        internal Device Device;
        internal ushort Identifier;
        internal ushort Length;

        internal int Offset;

        internal Reader Reader;
        internal ushort Version;

        internal Message(Device Device)
        {
            this.Device = Device;
            Data = new List<byte>(Constants.SendBuffer);
        }

        internal Message(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal byte[] ToBytes
        {
            get
            {
                var Packet = new List<byte>();

                Packet.AddUShort(Identifier);
                Packet.Add(0);
                Packet.AddUShort(Length);
                Packet.AddUShort(Version);
                Packet.AddRange(Data);

                return Packet.ToArray();
            }
        }

        internal virtual void Decode()
        {
        }

        internal virtual void Encode()
        {
        }

        internal virtual void Process()
        {
        }

        internal virtual void Decrypt()
        {
            if (Device.PlayerState >= State.LOGGED)
            {
                Device.Keys.SNonce.Increment();

                var Decrypted = Sodium.Decrypt(new byte[16].Concat(Reader.ReadBytes(Length)).ToArray(),
                    Device.Keys.SNonce, Device.Keys.PublicKey);

                if (Decrypted == null)
                    throw new CryptographicException("Tried to decrypt an incomplete message.");

                Reader = new Reader(Decrypted);
                Length = (ushort) Reader.BaseStream.Length;
            }
        }

        internal virtual void Encrypt()
        {
            if (Device.PlayerState >= State.LOGGED)
            {
                Device.Keys.RNonce.Increment();

                Data = new List<byte>(Sodium.Encrypt(Data.ToArray(), Device.Keys.RNonce, Device.Keys.PublicKey)
                    .Skip(16)
                    .ToArray());
            }

            Length = (ushort) Data.Count;
        }
    }
}