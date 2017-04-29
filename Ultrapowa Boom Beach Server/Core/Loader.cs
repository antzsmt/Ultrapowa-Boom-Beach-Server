namespace UCS.Core
{
    using Core.Checker;
    using Core.Events;
    using Core.Settings;
    using Core.Threading;
    using Database;
    using Helpers;
    using Packets;
    using WebAPI;

    internal class Loader
    {
        internal API API;
        internal CommandFactory CommandFactory;
        internal CSVManager Csv;
        internal DirectoryChecker DirectoryChecker;
        internal EventsHandler Events;
        internal Logger Logger;
        internal MemoryThread MemThread;
        internal MessageFactory MessageFactory;
        internal ObjectManager ObjectManager;
        internal ParserThread Parser;
        internal Redis Redis;
        internal ResourcesManager ResourcesManager;

        public Loader()
        {
            // CSV Files and Logger
            Logger = new Logger();
            DirectoryChecker = new DirectoryChecker();
            Csv = new CSVManager();

            if (Utils.ParseConfigBoolean("UseWebAPI"))
                API = new API();

            //Core
            ResourcesManager = new ResourcesManager();
            ObjectManager = new ObjectManager();
            Events = new EventsHandler();
            if (Constants.UseCacheServer)
                Redis = new Redis();


            CommandFactory = new CommandFactory();

            MessageFactory = new MessageFactory();

            // Optimazions
            MemThread = new MemoryThread();

            // User
            Parser = new ParserThread();
        }
    }
}