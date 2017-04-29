namespace UCS.Packets.Commands.Client
{
    using Core.Network;
    using Helpers.Binary;
    using Packets.Messages.Server;

    internal class CancelBattleCommand : Command
    {
        public CancelBattleCommand(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {
        }

        internal override void Decode()
        {
        }

        internal override void Process()
        {
            new OwnHomeDataMessage(Device).Send();
        }
    }
}