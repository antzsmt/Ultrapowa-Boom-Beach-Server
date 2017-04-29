namespace UCS.Packets
{
    using System;
    using System.Collections.Generic;
    using Packets.Messages.Client;

    internal class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

        public MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(SessionRequest)},
                {10101, typeof(Authentication)},
                {10212, typeof(ChangeName)},
                {10108, typeof(KeepAliveMessage)},
                {14101, typeof(GoHomeMessage)},
                {14102, typeof(ExecuteCommandsMessage)},
                {14113, typeof(VisitPlayer)},
                {14165, typeof(AttackNPC)},
                {14403, typeof(TopGlobalPlayers)},
                {14404, typeof(TopLocalPlayers)},
                {14405, typeof(RequestAvatarStream)}
            };
            //14401
            //14358
        }
    }
}