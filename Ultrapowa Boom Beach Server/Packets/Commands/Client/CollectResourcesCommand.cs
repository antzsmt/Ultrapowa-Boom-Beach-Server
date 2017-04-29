namespace UCS.Packets.Commands.Client
{
    using Helpers.Binary;

    internal class CollectResourcesCommand : Command
    {
        internal int BuildingID;

        public CollectResourcesCommand(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
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