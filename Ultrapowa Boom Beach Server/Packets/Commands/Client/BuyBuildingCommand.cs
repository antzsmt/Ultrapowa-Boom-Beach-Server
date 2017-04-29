namespace UCS.Packets.Commands.Client
{
    using Helpers.Binary;

    internal class BuyBuildingCommand : Command
    {
        internal int BuildingID;
        internal int X;
        internal int Y;

        public BuyBuildingCommand(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.BuildingID = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            /*System.Console.WriteLine($"X  -> {this.X}");
            System.Console.WriteLine($"Y  -> {this.Y}");
            System.Console.WriteLine($"ID -> {this.BuildingID}");*/
        }
    }
}