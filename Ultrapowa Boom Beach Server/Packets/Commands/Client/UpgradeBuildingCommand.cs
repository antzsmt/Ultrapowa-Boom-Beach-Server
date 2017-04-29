namespace UCS.Packets.Commands.Client
{
    using Helpers.Binary;

    internal class UpgradeBuildingCommand : Command
    {
        internal int BuildingID;

        public UpgradeBuildingCommand(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
        }
    }
}