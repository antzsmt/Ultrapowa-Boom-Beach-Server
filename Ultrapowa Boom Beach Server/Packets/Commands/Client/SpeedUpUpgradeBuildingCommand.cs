namespace UCS.Packets.Commands.Client
{
    using Helpers.Binary;

    internal class SpeedUpUpgradeBuildingCommand : Command
    {
        internal int BuildingID;

        public SpeedUpUpgradeBuildingCommand(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {
        }

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            //Console.WriteLine($"ID -> {this.BuildingID}");
        }
    }
}