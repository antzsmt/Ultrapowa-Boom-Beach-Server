namespace UCS.Core
{
    using System;
    using Core.Network;

    internal class Resources
    {
        internal static Random Random;
        internal static Gateway Gateway;

        internal static DatabaseManager DatabaseManager;
        internal static Loader Loader;


        internal static void Initialize()
        {
            Loader = new Loader();
            Random = new Random();
            Gateway = new Gateway();
            DatabaseManager = new DatabaseManager();
        }
    }
}