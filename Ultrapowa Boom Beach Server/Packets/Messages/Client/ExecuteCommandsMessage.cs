namespace UCS.Packets.Messages.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Helpers.Binary;

    // Packet 14102
    internal class ExecuteCommandsMessage : Message
    {
        internal int Checksum;
        internal byte[] Commands;
        internal int Count;
        internal List<Command> LCommands;

        public ExecuteCommandsMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt64();
            this.Reader.ReadByte();
            this.Count = this.Reader.ReadInt32();
            this.LCommands = new List<Command>(Count);
            this.Commands = this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {
            this.Device.Player.Tick();

            if (Count > -1 && Count <= 400)
            {
                using (Reader Reader = new Reader(Commands))
                {
                    for (int _Index = 0; _Index < Count; _Index++)
                    {
                        int CommandID = Reader.ReadInt32();
                        if (CommandFactory.Commands.ContainsKey(CommandID))
                        {
                            Logger.Write($"Command '{ CommandID }' is handled");

                            Command Command = Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, Device, CommandID) as Command;

                            if (Command != null)
                            {
                                Command.Decode();
                                Command.Process();

                                LCommands.Add(Command);
                            }
                        }
                        else
                        {
                            Logger.Write($"Command { CommandID } has not been handled.");
                            if (LCommands.Any())Logger.Write($"Previous command was { LCommands.Last().Identifier }. [{_Index + 1} / { Count }]");
                            break;
                        }
                    }
                }
            }
        }
    }
}