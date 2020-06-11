using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class LootContainerSummary
    {
        public string Name;
        public int ItemID;

        public List<DM_Quantity> All_Locations = new List<DM_Quantity>();
        public List<DroptableSummary> DropTables = new List<DroptableSummary>();
    }
}
