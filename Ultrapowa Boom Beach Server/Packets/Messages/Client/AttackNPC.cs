namespace UCS.Packets.Messages.Client
{
    using System;
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    internal class AttackNPC : Message
    {
        internal int LevelID;

        public AttackNPC(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.LevelID = Reader.ReadInt32();
        }

        internal override void Process()
        {
            new NPC_Data(this.Device) { /*LevelID = this.LevelID */ }.Send();
        }
    }
}