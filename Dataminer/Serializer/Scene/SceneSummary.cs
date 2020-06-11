using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class SceneSummary
    {
        public string SceneName;

        public List<DM_Quantity> Enemies = new List<DM_Quantity>();
        public List<string> Merchants = new List<string>();

        public List<DM_Quantity> Loot_Containers = new List<DM_Quantity>();
        public List<DM_Quantity> Gatherables = new List<DM_Quantity>();
        public List<DM_ItemSpawn> Item_Spawns = new List<DM_ItemSpawn>();

        public List<string> UniqueContainerList = new List<string>();
    }
}
