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
        internal API API                           = null;
        internal CommandFactory CommandFactory     = null;
        internal CSVManager Csv                    = null;
        internal DirectoryChecker DirectoryChecker = null;
        internal EventsHandler Events              = null;
        internal Logger Logger                     = null;
        internal MemoryThread MemThread            = null;
        internal MessageFactory MessageFactory     = null;
        internal ObjectManager ObjectManager       = null;
        internal ParserThread Parser               = null;
        internal Redis Redis                       = null;
        internal ResourcesManager ResourcesManager = null;
        internal Settings.Settings Settings        = null;

        public Loader()
        {
            // CSV Files and Logger
            Logger = new Logger();
            DirectoryChecker = new DirectoryChecker();
            Csv = new CSVManager();
            Settings = new Settings.Settings();

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