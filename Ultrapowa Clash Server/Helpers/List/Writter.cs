namespace UCS.Helpers.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core;
    using Logic;
    using Utilities.ZLib;

    internal static class Writer
    {
        public static void AddByte(this List<byte> _Packet, int _Value)
        {
            _Packet.Add((byte)_Value);
        }

        public static void AddInt(this List<byte> _Packet, int _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse());
        }

        public static void AddIntEndian(this List<byte> _Packet, int _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value));
        }

        public static void AddInt(this List<byte> _Packet, int _Value, int _Skip)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse().Skip(_Skip));
        }

        public static void AddLong(this List<byte> _Packet, long _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse());
        }

        public static void AddLong(this List<byte> _Packet, long _Value, int _Skip)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse().Skip(_Skip));
        }

        public static void AddBool(this List<byte> _Packet, bool _Value)
        {
            _Packet.Add(_Value ? (byte)1 : (byte)0);
        }

        public static void AddString(this List<byte> _Packet, string _Value)
        {
            if (_Value == null)
            {
                _Packet.AddRange(BitConverter.GetBytes(-1).Reverse());
            }
            else
            {
                byte[] _Buffer = Encoding.UTF8.GetBytes(_Value);

                _Packet.AddInt(_Buffer.Length);
                _Packet.AddRange(_Buffer);
            }
        }

        public static void AddByteArray(this List<byte> list, byte[] data)
        {
            if (data == null)
                list.AddInt(-1);
            else
            {
                list.AddInt(data.Length);
                list.AddRange(data);
            }
        }

        public static void AddUShort(this List<byte> _Packet, ushort _Value)
        {
            _Packet.AddRange(BitConverter.GetBytes(_Value).Reverse());
        }

        public static void AddCompressed(this List<byte> _Packet, string _Value, bool addbool = true)
        {
            if (addbool)
                _Packet.AddBool(true);

                byte[] Compressed = ZlibStream.CompressString(_Value);

                _Packet.AddInt(Compressed.Length + 4);
                _Packet.AddRange(BitConverter.GetBytes(_Value.Length));
                _Packet.AddRange(Compressed);
        }

        public static void AddHexa(this List<byte> _Packet, string _Value)
        {
            _Packet.AddRange(_Value.HexaToBytes());
        }

        public static byte[] HexaToBytes(this string _Value)
        {
            string _Tmp = _Value.Replace("-", string.Empty);
            return Enumerable.Range(0, _Tmp.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(_Tmp.Substring(x, 2), 16)).ToArray();
        }

        internal static void Shuffle<T>(this IList<T> List)
        {
            int c = List.Count;

            while (c > 1)
            {
                c--;

                int r = Resources.Random.Next(c + 1);

                T Value = List[r];
                List[r] = List[c];
                List[c] = Value;
            }
        }
    }
}