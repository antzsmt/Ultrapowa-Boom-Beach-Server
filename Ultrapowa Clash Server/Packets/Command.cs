using System.Collections.Generic;
using UCS.Helpers.Binary;

namespace UCS.Packets
{
    internal class Command
    {
        public const int MaxEmbeddedDepth = 10;

        internal List<byte> Data;

        internal int Depth;
        internal Device Device;

        internal int Identifier;

        internal Reader Reader;

        internal int SubTick1;
        internal int SubTick2;

        internal Command(Device Device)
        {
            this.Device = Device;
            Data = new List<byte>();
        }

        internal Command(Reader Reader, Device Device, int Identifier)
        {
            this.Identifier = Identifier;
            this.Device = Device;
            this.Reader = Reader;
        }

        internal virtual void Decode()
        {
            // Decode.
        }

        internal virtual void Encode()
        {
            // Encode.
        }

        internal virtual void Process()
        {
            // Process.
        }
    }
}