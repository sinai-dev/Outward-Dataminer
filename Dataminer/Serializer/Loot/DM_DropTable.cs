using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using SideLoader;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_DropTable
    {
        public string Name;

        public List<DropTableEntry> Guaranteed_Drops = new List<DropTableEntry>();
        public List<DropGeneratorHolder> Random_Tables = new List<DropGeneratorHolder>();

        public static DM_DropTable ParseDropable(Dropable dropable, Merchant merchant = null, string containerName = "")
        {
            if (!dropable)
                return null;

            var dropTableHolder = new DM_DropTable
            {
                Name = dropable.name
            };

            if (dropTableHolder.Name == "InventoryTable" && merchant)
            {
                dropTableHolder.Name = SceneManager.Instance.GetCurrentLocation(merchant.transform.position) + " - " + merchant.ShopName;
            }

            dropTableHolder.Name = dropTableHolder.Name.Trim();

            if (At.GetField(dropable, "m_allGuaranteedDrops") is List<GuaranteedDrop> guaranteedDrops)
            {
                foreach (GuaranteedDrop gDropper in guaranteedDrops)
                {
                    if (At.GetField(gDropper, "m_itemDrops") is List<BasicItemDrop> gItemDrops && gItemDrops.Count > 0)
                    {
                        foreach (BasicItemDrop gItemDrop in gItemDrops)
                        {
                            if (!gItemDrop.DroppedItem)
                                continue;

                            //var pos = dropable.transform.position;
                            AddGuaranteedDrop(dropTableHolder,
                                gItemDrop.DroppedItem.ItemID,
                                gItemDrop.DroppedItem.Name,
                                gItemDrop.MinDropCount,
                                gItemDrop.MaxDropCount);
                        }
                    }
                }
            }

            if (At.GetField(dropable, "m_conditionalGuaranteedDrops") is List<ConditionalGuaranteedDrop> conditionalGuaranteed)
            {
                foreach (var guaConditional in conditionalGuaranteed)
                {
                    if (!guaConditional.Dropper)
                        continue;

                    var drops = At.GetField(guaConditional.Dropper, "m_itemDrops") as List<BasicItemDrop>;
                    foreach (var gItemDrop in drops)
                    {
                        if (!gItemDrop.DroppedItem)
                            continue;

                        //var pos = dropable.transform.position;
                        AddGuaranteedDrop(dropTableHolder,
                            gItemDrop.DroppedItem.ItemID,
                            gItemDrop.DroppedItem.Name,
                            gItemDrop.MinDropCount,
                            gItemDrop.MaxDropCount);
                    }
                }
            }

            if (At.GetField(dropable, "m_mainDropTables") is List<DropTable> dropTables)
            {
                foreach (DropTable table in dropTables)
                {
                    if (ParseDropTable(dropTableHolder, containerName, dropable, table) is DropGeneratorHolder generatorHolder)
                    {
                        dropTableHolder.Random_Tables.Add(generatorHolder);
                    }
                }
            }

            if (At.GetField(dropable, "m_conditionalDropTables") is List<ConditionalDropTable> conditionals)
            {
                foreach (var conTable in conditionals)
                {
                    if (ParseDropTable(dropTableHolder, containerName, dropable, conTable.Dropper) is DropGeneratorHolder generatorHolder)
                    {
                        generatorHolder.DropConditionType = conTable.DropCondition.GetType().FullName;

                        dropTableHolder.Random_Tables.Add(generatorHolder);
                    }
                }
            }

            if (merchant == null)
            {
                string dir = Serializer.Folders.DropTables;
                if (!File.Exists(dir + "/" + dropTableHolder.Name + ".xml"))
                {
                    ListManager.DropTables.Add(dropTableHolder.Name, dropTableHolder);
                    Serializer.SaveToXml(dir, dropTableHolder.Name, dropTableHolder);
                }
            }

            return dropTableHolder;
        }

        private static void AddGuaranteedDrop(DM_DropTable dropTableHolder, int item_ID, string item_Name, int min_Qty, int max_Qty)
        {
            // check if we already have this guaranteed drop, if so add to quantity
            bool newDrop = true;
            foreach (DropTableEntry gDropHolder in dropTableHolder.Guaranteed_Drops)
            {
                if (item_ID == gDropHolder.Item_ID)
                {
                    newDrop = false;
                    gDropHolder.Min_Quantity += min_Qty;
                    gDropHolder.Max_Quantity += max_Qty;
                }
            }
            if (newDrop)
            {
                if (IsUnidentified(item_ID, out string identified))
                    item_Name += " (" + identified + ")";

                dropTableHolder.Guaranteed_Drops.Add(new DropTableEntry
                {
                    Item_Name = item_Name,
                    Item_ID = item_ID,
                    Min_Quantity = min_Qty,
                    Max_Quantity = max_Qty
                });
            }
        }

        public static DropGeneratorHolder ParseDropTable(DM_DropTable dropTableHolder, string containerName, Dropable dropper, DropTable table)
        {
            var generatorHolder = new DropGeneratorHolder
            {
                MinNumberOfDrops = table.MinNumberOfDrops,
                MaxNumberOfDrops = table.MaxNumberOfDrops,
                MaxDiceValue = (int)At.GetField(table, "m_maxDiceValue"),
            };

            //if (generatorHolder.MaxDiceValue <= 0)
            //{
            //    SL.LogWarning("This random table's MaxDiceValue is 0, skipping...");
            //    return null;
            //}

            if (At.GetField(table, "m_dropAmount") is SimpleRandomChance dropAmount)
            {
                generatorHolder.ChanceReduction = dropAmount.ChanceReduction;
                generatorHolder.ChanceRegenQty = dropAmount.ChanceRegenQty;

                if (dropAmount.CanRegen)
                    generatorHolder.RegenTime = dropAmount.ChanceRegenDelay;
                else
                    generatorHolder.RegenTime = -1;
            }
            else
            {
                generatorHolder.RegenTime = -1;
            }

            if (At.GetField(table, "m_emptyDropChance") is int i && generatorHolder.MaxDiceValue > 0)
            {
                try
                {
                    decimal emptyChance = (decimal)i / generatorHolder.MaxDiceValue;
                    generatorHolder.EmptyDrop = (float)emptyChance * 100;
                }
                catch
                {
                    generatorHolder.EmptyDrop = 0;
                }
            }

            if (At.GetField(table, "m_itemDrops") is List<ItemDropChance> itemDrops)
            {
                foreach (ItemDropChance drop in itemDrops)
                {
                    if (!drop.DroppedItem)
                        continue;

                    float percentage;
                    try
                    {
                        percentage = (float)((decimal)drop.DropChance / generatorHolder.MaxDiceValue) * 100f;
                    }
                    catch
                    {
                        percentage = 100f;
                    }

                    percentage = (float)Math.Round(percentage, 2);

                    string name = drop.DroppedItem.Name;

                    if (IsUnidentified(drop.DroppedItem.ItemID, out string identified))
                        name += " (" + identified + ")";

                    if (percentage >= 100)
                    {
                        AddGuaranteedDrop(dropTableHolder, drop.DroppedItem.ItemID, name, drop.MaxDropCount, drop.MinDropCount);
                    }
                    else
                    {
                        generatorHolder.Item_Drops.Add(new DropTableChanceEntry
                        {
                            Item_ID = drop.DroppedItem.ItemID,
                            Item_Name = name,
                            Min_Quantity = drop.MinDropCount,
                            Max_Quantity = drop.MaxDropCount,
                            Drop_Chance = percentage,
                            Dice_Range = drop.MaxDiceRollValue - drop.MinDiceRollValue,
                            ChanceReduction = drop.ChanceReduction,
                            ChanceRegenDelay = drop.ChanceRegenDelay,
                            ChanceRegenQty = drop.ChanceRegenQty
                        });
                    }
                }
            }

            return generatorHolder;
        }

        public static bool IsUnidentified(int id, out string identified)
        {
            if (s_identifiedSamples.ContainsKey(id))
            {
                identified = s_identifiedSamples[id];
                return true;
            }
            else
            {
                identified = "N/A";
                return false;
            }
        }

        internal static Dictionary<int, string> s_identifiedSamples = new Dictionary<int, string>
        {
            { 6000340, "1000 Funds" },
            { 6000341, "40 Stones" },
            { 6000342, "80 Stones" },
            { 6000343, "Diamond Dust" },
            { 6000344, "Chromium Shards" },
            { 6000345, "Amethyst Geode" },
            { 6000380, "1000 Funds" },
            { 6000381, "40 Timber" },
            { 6000382, "80 Timber" },
            { 6000383, "Flash Moss" },
            { 6000384, "Bloodroot" },
            { 6000385, "Voltaic Vines" },
            { 6000420, "1000 Funds" },
            { 6000421, "200 Food" },
            { 6000422, "300 Food" },
            { 6000423, "Ectoplasm" },
            { 6000424, "Digested Mana Stone" },
            { 6000425, "Petrified Organs" },
        };

        public class DropGeneratorHolder
        {
            public int MinNumberOfDrops;
            public int MaxNumberOfDrops;
            public int MaxDiceValue;
            public float EmptyDrop;
            
            public float RegenTime;
            public int ChanceReduction;
            public int ChanceRegenQty;

            public List<DropTableChanceEntry> Item_Drops = new List<DropTableChanceEntry>();

            public string DropConditionType;
        }
    }
}
