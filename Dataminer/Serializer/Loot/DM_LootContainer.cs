using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_LootContainer
    {
        public string Name;
        public int ItemID;
        public string UID;

        public List<string> DropTables = new List<string>();

        public static DM_LootContainer ParseLootContainer(TreasureChest loot)
        {
            var lootHolder = new DM_LootContainer
            {
                Name = loot.Name,
                ItemID = loot.ItemID,
                UID = loot.UID
            };

            if (lootHolder.Name == "Pocket")
            {
                lootHolder.Name = "Corpse";
            }

            if (At.GetField(loot as SelfFilledItemContainer, "m_drops") is List<Dropable> droppers)
            {
                foreach (Dropable dropper in droppers)
                {
                    var dropableHolder = DM_DropTable.ParseDropable(dropper, null, lootHolder.Name);
                    lootHolder.DropTables.Add(dropableHolder.Name);
                }
            }

            string dir = Serializer.Folders.Scenes + "/" + SceneManager.Instance.GetCurrentRegion() + "/" + SceneManager.Instance.GetCurrentLocation(loot.transform.position);
            string saveName = lootHolder.Name + "_" + lootHolder.UID;
            Serializer.SaveToXml(dir + "/LootContainers", saveName, lootHolder);

            return lootHolder;
        }
    }
}
