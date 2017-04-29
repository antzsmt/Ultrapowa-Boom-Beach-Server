namespace UCS.Core.Network
{
    using System;
    using Helpers;
    using Packets;

    internal static class Processor
    {
        internal static void Recept(this Message Message)
        {
            Message.Decrypt();
            Message.Decode();
            Message.Process();
        }

        internal static void Send(this Message Message)
        {
            try
            {
                Message.Encode();
                Message.Encrypt();
                Resources.Gateway.Send(Message);

                Logger.Write($"Message {Message.Identifier} has been sent.");

                Message.Process();
            }
            catch (Exception)
            {
            }
        }

        internal static Command Handle(this Command Command)
        {
            Command.Encode();

            return Command;
        }
    }
}