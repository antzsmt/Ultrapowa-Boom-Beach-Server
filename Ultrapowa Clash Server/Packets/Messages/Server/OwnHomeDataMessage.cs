namespace UCS.Packets.Messages.Server
{
    using System;
    using Core;
    using Helpers.List;
    using Logic;
    using Logic.Enums;

    internal class OwnHomeDataMessage : Message
    {
        public OwnHomeDataMessage(Device client) : base(client)
        {
            this.Identifier = 24101;
        }

        internal override async void Encode()
        {
            ClientHome Home = new ClientHome();
            Home.Village = ObjectManager.HomeDefault /*this.Device.Player.SaveToJSON()*/;

            //Console.WriteLine(this.Device.Player.SaveToJSON());

            this.Data.AddInt(0);
            this.Data.AddLong(DateTime.UtcNow.Ticks);
            this.Data.AddRange(Home.Encode);
            this.Data.AddRange(this.Device.Player.Avatar.Encode);
        }
    }
}