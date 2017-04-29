namespace UCS.Packets
{
    using System;

    internal class Crypto : IDisposable
    {
        internal byte[] PublicKey;
        internal byte[] RNonce;
        internal byte[] SNonce;

        internal Crypto()
        {
            PublicKey = new byte[32];
            SNonce = new byte[24];
            RNonce = new byte[24];
        }

        void IDisposable.Dispose()
        {
            SNonce = null;
            RNonce = null;
            PublicKey = null;
        }
    }
}