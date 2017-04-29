namespace UCS.Packets.Commands.Client
{
    using Helpers.Binary;

    internal class MoveBuildingCommand : Command
    {
        internal int BuildingID;
        internal int X;
        internal int Y;

        public MoveBuildingCommand(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
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
            /* Console.WriteLine($"X  -> {this.X}");
             Console.WriteLine($"Y  -> {this.Y}");
             Console.WriteLine($"ID -> {this.BuildingID}");*/
        }
    }
}