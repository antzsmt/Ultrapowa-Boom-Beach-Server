namespace UCS.Logic
{
    using System.Collections.Generic;
    using System.IO;
    using Helpers;
    using Helpers.Binary;
    using Helpers.List;

    internal class Base
    {
        public Base()
        {
        }

        public virtual void Decode(byte[] baseData)
        {
        }

        public virtual byte[] Encode
        {
            get
            {
                List<byte> data = new List<byte>();
                return data.ToArray();
            }
        }
    }
}
