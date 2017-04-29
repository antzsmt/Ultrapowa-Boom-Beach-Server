using UCS.Core;
using UCS.Files;

namespace UCS.Logic
{
    internal class Building : ConstructionItem
    {
        public Building(Data _Data, Level level) : base(_Data, level)
        {
            
        }

        public override int ClassId => 0;

        public Buildings GetBuildingData() => (Buildings)Data;
    }
}