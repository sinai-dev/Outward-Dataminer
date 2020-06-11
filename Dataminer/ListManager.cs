using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

namespace Dataminer
{
    public class ListManager : MonoBehaviour
    {
        public static ListManager Instance;

        // Scene Summary dictionary
        public static Dictionary<string, SceneSummary> SceneSummaries = new Dictionary<string, SceneSummary>();

        // Tag Sources
        public static Dictionary<string, List<string>> TagSources = new Dictionary<string, List<string>>();

        //// Item Loot Sources (spawns and loot containers)
        //public static Dictionary<string, ItemSource> ItemLootSources = new Dictionary<string, ItemSource>();

        // Container Summaries
        public static Dictionary<string, LootContainerSummary> ContainerSummaries = new Dictionary<string, LootContainerSummary>();

        // Lists
        public static Dictionary<string, List<DM_Enemy>> EnemyManifest = new Dictionary<string, List<DM_Enemy>>();
        public static Dictionary<string, DM_Merchant> Merchants = new Dictionary<string, DM_Merchant>();
        public static Dictionary<string, DM_Item> Items = new Dictionary<string, DM_Item>();

        public static Dictionary<string, DM_StatusEffect> Effects = new Dictionary<string, DM_StatusEffect>();
        public static Dictionary<string, DM_ImbueEffect> ImbueEffects = new Dictionary<string, DM_ImbueEffect>();

        public static Dictionary<string, DM_Recipe> Recipes = new Dictionary<string, DM_Recipe>();
        public static Dictionary<string, DM_DropTable> DropTables = new Dictionary<string, DM_DropTable>();

        internal void Awake()
        {
            Instance = this;
        }

        //internal void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.ScrollLock))
        //    {
        //        Debug.Log("Force saving lists");
        //        SaveLists();
        //    }
        //}

        public static string GetSceneSummaryKey(Vector3 position)
        {
            return SceneManager.Instance.GetCurrentRegion() + ":" + SceneManager.Instance.GetCurrentLocation(position);
        }

        public static void AddTagSource(Tag tag, string source)
        {
            string key = tag.ToString();

            if (!TagSources.ContainsKey(key))
            {
                TagSources.Add(key, new List<string> { });
            }

            if (!string.IsNullOrEmpty(source))
            {
                TagSources[key].Add(source);
            }
        }

        public static void AddContainerSummary(string name, string location, List<string> dropTables, string scene)
        {
            if (ContainerSummaries.ContainsKey(name))
            {
                var summary = ContainerSummaries[name];

                bool addLocation = true;
                foreach (var loc in summary.All_Locations)
                {
                    if (loc.Name == location)
                    {
                        loc.Quantity++;
                        addLocation = false;
                        break;
                    }
                }
                if (addLocation)
                {
                    summary.All_Locations.Add(new DM_Quantity
                    {
                        Name = location,
                        Quantity = 1
                    });
                }
            }
            else
            {
                ContainerSummaries.Add(name, new LootContainerSummary
                {
                    Name = name,
                    All_Locations = new List<DM_Quantity>
                    {
                        new DM_Quantity
                        {
                            Name = location,
                            Quantity = 1
                        }
                    },
                    DropTables = new List<DroptableSummary>()
                });
            }
            foreach (var table in dropTables)
            {
                bool addNew = true;
                foreach (var holder in ContainerSummaries[name].DropTables)
                {
                    if (holder.DropTableName == table)
                    {
                        addNew = false;
                        if (!holder.Locations.Contains(scene))
                        {
                            holder.Locations.Add(scene);
                        }
                        break;
                    }
                }
                if (addNew)
                {
                    ContainerSummaries[name].DropTables.Add(new DroptableSummary
                    {
                        DropTableName = table,
                        Locations = new List<string>
                        {
                            scene
                        }
                    });
                }
            }
        }

        public static void SaveLists()
        {
            // ========== Scene Summaries ==========
            foreach (var entry in SceneSummaries)
            {
                string[] array = entry.Key.Split(new char[] { ':' });
                string region = array[0];
                string location = array[1];

                string dir = Serializer.Folders.Scenes + "/" + region + "/" + location;
                string saveName = "Summary";
                Serializer.SaveToXml(dir, saveName, entry.Value);
            }

            // ========== Tag Sources ==========
            List<string> TagTable = new List<string>();
            foreach (var entry in TagSources)
            {
                string s = "";
                foreach (var source in entry.Value)
                {
                    if (s != "") { s += ","; }
                    s += source;
                }
                TagTable.Add(entry.Key + "	" + s);
            }
            File.WriteAllLines(Serializer.Folders.Lists + "/TagSources.txt", TagTable.ToArray());

            //// ========== Item Sources ==========
            //List<string> ItemSourcesTable = new List<string>();
            //foreach (var entry in ItemLootSources)
            //{
            //    string dir = Folders.Lists + "/ItemSources";
            //    Dataminer.SerializeXML(dir, entry.Key, entry.Value, typeof(ItemSource));

            //    ItemSourcesTable.Add(entry.Key + "	" + entry.Value.ItemName);
            //}
            //File.WriteAllLines(Folders.Lists + "/ItemSources.txt", ItemSourcesTable.ToArray());

            // ========== Container Sources ==========
            foreach (var entry in ContainerSummaries)
            {
                string dir = Serializer.Folders.Lists + "/ContainerSummaries";
                Serializer.SaveToXml(dir, entry.Key, entry.Value);
            }
            File.WriteAllLines(Serializer.Folders.Lists + "/ContainerSummaries.txt", ContainerSummaries.Keys.ToArray());

            // ========== Enemies ==========
            List<string> EnemyTable = new List<string>();
            foreach (var entry in EnemyManifest)
            {
                foreach (var enemyHolder in entry.Value)
                {
                    EnemyTable.Add(enemyHolder.Name + " (" + enemyHolder.Unique_ID + ")");
                }
            }
            File.WriteAllLines(Serializer.Folders.Lists + "/Enemies.txt", EnemyTable.ToArray());

            // ========== Merchants ===========
            List<string> MerchantTable = new List<string>();
            foreach (var entry in Merchants)
            {
                MerchantTable.Add(entry.Key);
            }
            File.WriteAllLines(Serializer.Folders.Lists + "/Merchants.txt", MerchantTable.ToArray());

            // ========== Items ==========
            List<string> ItemTable = new List<string>();
            foreach (var entry in Items)
            {
                string saveDir = entry.Value.saveDir;
                if (string.IsNullOrEmpty(saveDir))
                {
                    if (entry.Value.Tags.Contains("Consummable"))
                    {
                        saveDir = "/Consumable";
                    }
                    else
                    {
                        saveDir = "/_Unsorted";
                    }
                }

                ItemTable.Add(entry.Key + "	" + entry.Value.Name + "	" + entry.Value.gameObjectName + "	" + saveDir);

            }
            File.WriteAllLines(Serializer.Folders.Lists + "/Items.txt", ItemTable.ToArray());

            // ========== Effects ==========
            List<string> EffectsTable = new List<string>();
            foreach (var entry in Effects)
            {
                EffectsTable.Add(entry.Key + "	" + entry.Value.Name);
            }
            foreach (var entry in ImbueEffects)
            {
                EffectsTable.Add(entry.Key + "	" + entry.Value.Name);
            }
            File.WriteAllLines(Serializer.Folders.Lists + "/Effects.txt", EffectsTable.ToArray());

            // ========== Recipes ==========
            List<string> RecipesTable = new List<string>();
            foreach (var entry in Recipes)
            {
                string results = "";
                foreach (var result in entry.Value.Results)
                {
                    if (results != "") { results += ","; }
                    results += result.ItemName + " (" + result.Quantity + ")";
                }

                string ingredients = "";
                foreach (var ingredient in entry.Value.Ingredients)
                {
                    if (ingredients != "") { ingredients += ","; }
                    ingredients += ingredient;
                }

                RecipesTable.Add(entry.Key + "	" + entry.Value.StationType + "	" + results + "	" + ingredients);
            }
            File.WriteAllLines(Serializer.Folders.Lists + "/Recipes.txt", RecipesTable.ToArray());

            // ========== DropTables ==========
            File.WriteAllLines(Serializer.Folders.Lists + "/DropTables.txt", DropTables.Keys.ToArray());

            Debug.Log("[Dataminer] List building complete!");
        }
    }
}
