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

        public List<QuantityHolder> Enemies = new List<QuantityHolder>();
        public List<string> Merchants = new List<string>();

        public List<QuantityHolder> Loot_Containers = new List<QuantityHolder>();
        public List<QuantityHolder> Gatherables = new List<QuantityHolder>();
        public List<DM_ItemSpawn> Item_Spawns = new List<DM_ItemSpawn>();

        public List<string> UniqueContainerList = new List<string>();
    }
}
