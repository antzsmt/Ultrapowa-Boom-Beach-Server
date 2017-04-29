namespace UCS.Logic
{
    using System.Collections.Generic;
    using Helpers.List;

    internal class ClientHome : Base
    {
        internal string Village;

        public ClientHome()
        {
        }

        public override byte[] Encode
        {
            get
            {
                List<byte> _Data = new List<byte>();

                _Data.AddString("layout/playerbase.level");
                _Data.AddCompressed(Village);

                return _Data.ToArray();
            }
        }
    }
}