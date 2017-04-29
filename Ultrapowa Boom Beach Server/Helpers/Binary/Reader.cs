namespace UCS.Helpers.Binary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public class Reader : BinaryReader
    {
        public Reader(byte[] _Buffer) : base(new MemoryStream(_Buffer))
        {

        }

        public bool EndOfStream => this.BaseStream.Length == this.BaseStream.Position;

        public override int Read(byte[] _Buffer, int _Offset, int _Count)
        {
            return this.BaseStream.Read(_Buffer, 0, _Count);
        }

        public byte[] ReadArray()
        {
            int _Length = this.ReadInt32();
            if (_Length == -1 || _Length < -1 || _Length > this.BaseStream.Length - this.BaseStream.Position)
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return _Buffer;
        }

        public override bool ReadBoolean()
        {
            byte state = this.ReadByte();
            switch (state)
            {
                case 0:
                    return false;

                case 1:
                    return true;

                default:
                    throw new NotSupportedException("Attempted to read a byte [" + state + "] but and convert it to boolean, but is out of boolean range.");
            }
        }

        public override byte ReadByte()
        {
            return (byte)this.BaseStream.ReadByte();
        }

        public byte[] ReadBytes()
        {
            int length = this.ReadInt32();
            if (length == -1)
            {
                return null;
            }

            return this.ReadBytes(length);
        }

        public override short ReadInt16()
        {
            return (short)this.ReadUInt16();
        }

        public int ReadInt24()
        {
            byte[] _Temp = this.ReadBytesWithEndian(3, false);
            return (_Temp[0] << 16) | (_Temp[1] << 8) | _Temp[2];
        }

        public override int ReadInt32()
        {
            return (int)this.ReadUInt32();
        }

        public override long ReadInt64()
        {
            return (long)this.ReadUInt64();
        }

        public override string ReadString()
        {
            int _Length = this.ReadInt32();
            if (_Length == -1 || _Length < -1 || _Length > this.BaseStream.Length - this.BaseStream.Position)
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return Encoding.UTF8.GetString(_Buffer);
        }

        public override ushort ReadUInt16()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(_Buffer, 0);
        }

        public uint ReadUInt24()
        {
            return (uint)this.ReadInt24();
        }

        public override uint ReadUInt32()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(_Buffer, 0);
        }

        public override ulong ReadUInt64()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(_Buffer, 0);
        }

        public long Seek(long _Offset, SeekOrigin _Origin = SeekOrigin.Current)
        {
            return this.BaseStream.Seek(_Offset, _Origin);
        }

        private byte[] ReadBytesWithEndian(int _Count, bool _Endian = true)
        {
            byte[] _Buffer = new byte[_Count];
            this.BaseStream.Read(_Buffer, 0, _Count);

            if (BitConverter.IsLittleEndian && _Endian)
            {
                Array.Reverse(_Buffer);
            }

            return _Buffer;
        }
    }
}