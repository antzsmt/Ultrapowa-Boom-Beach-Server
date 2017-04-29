namespace UCS.Files
{
    internal static class GlobalID
    {
        internal const int Reference = 1125899907;


        internal static int CreateGlobalID(int index, int count) => count + 1000000 * index;

        internal static int GetType(int GlobalID)
        {
            GlobalID = (int)((1125899907 * (long)GlobalID) >> 32);
            return (GlobalID >> 18) + (GlobalID >> 31);
        }

        internal static int GetID(int GlobalID)
        {
            int ReferenceT = 0;
            ReferenceT = (int)((1125899907 * (long)GlobalID) >> 32);
            return GlobalID - 1000000 * ((ReferenceT >> 18) + (ReferenceT >> 31));
        }
    }
}