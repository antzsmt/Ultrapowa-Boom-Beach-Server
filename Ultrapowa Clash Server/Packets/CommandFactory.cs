using System;
using System.Collections.Generic;
using UCS.Packets.Commands.Client;

namespace UCS.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {500, typeof(BuyBuildingCommand)},
                {501, typeof(MoveBuildingCommand)},
                {502, typeof(UpgradeBuildingCommand)},
                {504, typeof(SpeedUpUpgradeBuildingCommand)},
                {506, typeof(CollectResourcesCommand)},
                {603, typeof(CancelBattleCommand)}

                // 546 Save Village Layout
            };
        }
    }
}