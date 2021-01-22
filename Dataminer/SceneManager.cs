using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using HarmonyLib;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.DialogueTrees;
using SideLoader;

namespace Dataminer
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        private bool m_parsing;
        private Coroutine m_coroutine;

        internal void Awake()
        {
            Instance = this;

            SceneHelper.SetupSceneSummaries();
        }

        internal void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                DM_Enemy.ParseAllEnemies();
                //StartCoroutine(ForceSpawnAllBuildings());
            }

            if (Input.GetKeyDown(KeyCode.ScrollLock))
            {
                DM_Merchant.ParseAllMerchants();
            }

            if (Input.GetKeyDown(KeyCode.Pause))
            {
                if (!m_parsing)
                {
                    if (CharacterManager.Instance.GetFirstLocalCharacter() != null)
                    {
                        SL.Log("___________ Starting Scenes Parse ___________");
                        m_coroutine = StartCoroutine(ParseCoroutine());
                        m_parsing = true;
                    }
                }
                else
                {
                    m_parsing = false;

                    if (m_coroutine != null)
                    {
                        SL.Log("___________ Stopping Scenes Parse ___________");
                        StopCoroutine(m_coroutine);
                    }
                }
            }
        }

        private IEnumerator ParseCoroutine()
        {
            foreach (string sceneName in SceneHelper.SceneBuildNames.Keys)
            {
                /*        Load Scene        */

                if (SceneManagerHelper.ActiveSceneName != sceneName)
                {
                    SL.Log("--- Loading " + sceneName + " ---");

                    NetworkLevelLoader.Instance.RequestSwitchArea(sceneName, 0, 1.5f);

                    yield return new WaitForSeconds(5f);

                    while (NetworkLevelLoader.Instance.IsGameplayPaused || NetworkLevelLoader.Instance.InLoading)
                    {
                        NetworkLevelLoader loader = NetworkLevelLoader.Instance;
                        At.SetField(loader, "m_continueAfterLoading", true);

                        yield return new WaitForSeconds(1f);
                    }
                    yield return new WaitForSeconds(2f);
                    MenuManager.Instance.HideMasterLoadingScreen();
                }

                SL.Log("Parsing scene " + sceneName + "...");

                /*        Parse Scene        */

                //// Disable the TreeBehaviour Managers while we do stuff with enemies
                //DisableCanvases();

                // Parse Enemies
                SL.Log("Parsing enemies");
                DM_Enemy.ParseAllEnemies();

                // Parse Merchants
                SL.Log("Parsing merchants");
                DM_Merchant.ParseAllMerchants();

                // Parse Loot (+ item sources)
                SL.Log("Parsing loot");
                ParseAllLoot();

                // check if New Sirocco

                if (SceneManagerHelper.ActiveSceneName == "NewSirocco")
                {
                    SL.LogWarning("NEW SIROCCO: Dumping all buildings...");

                    StartCoroutine(DatamineBuildings());

                    while (!s_doneBuildings)
                        yield return null;

                    ClearScene();
                    yield return new WaitForSeconds(1f);
                }

                SL.Log("--- Finished Scene: " + SceneManagerHelper.ActiveSceneName + " ---");
            }

            SL.Log("___________ Finished Scenes Parse ___________");

            SL.Log("[Dataminer] Saving lists...");
            ListManager.SaveLists();

            SL.Log("[Dataminer] Finished.");
        }

        internal static bool s_doneBuildings;

        private IEnumerator DatamineBuildings()
        {
            var player = CharacterManager.Instance.GetFirstLocalCharacter();
            var buildingsObj = GameObject.Find("Buildings");

            var buildings = References.RPM_ITEM_PREFABS.Values.Where(it => it is Building);
            //var created = new List<Building>();
            foreach (Building prefab in buildings)
            {
                SL.Log("Checking prefab " + prefab.ItemID + ": " + prefab.Name);

                if (!(At.GetField(prefab, "m_upgradePhases") is Building.ConstructionPhase[] upgrades))
                {
                    SL.Log("m_upgradePhases was null");
                    continue;
                }

                ClearScene();
                yield return new WaitForSeconds(1f);

                MakeBuilding(prefab);

                yield return new WaitForSeconds(1f);

                DM_Merchant.ParseAllMerchants();
                ParseAllLoot();

                ClearScene();
                yield return new WaitForSeconds(1f);

                if (upgrades.Length > 0)
                {
                    for (int i = 0; i < upgrades.Length; i++)
                    {
                        MakeBuilding(prefab, i);
                        yield return new WaitForSeconds(1f);

                        DM_Merchant.ParseAllMerchants();
                        ParseAllLoot();

                        ClearScene();
                        yield return new WaitForSeconds(1f);
                    }
                }
            }

            yield return new WaitForSeconds(1f);

            s_doneBuildings = true;

            GameObject MakeBuilding(Building prefab, int upgradePhase = -1)
            {
                var building = ItemManager.Instance.GenerateItemNetwork(prefab.ItemID) as Building;

                At.SetField(building, "m_currentUpgradePhaseIndex", upgradePhase);

                At.SetField(building, "m_remainingConstructionTime", 0);
                At.SetField(building, "m_pendingUpgradePhaseIndex", -1);

                building.UpdateConstruction(0f);
                //At.Invoke(building, "ActivateLevelVisuals", Enumerable.Empty<Type>());

                building.transform.parent = buildingsObj.transform;
                //building.transform.position = player.transform.position;

                //created.Add(building);
                SL.LogWarning("Activated building " + building.Name + " (" + building.ItemID + "), phase: " + upgradePhase);

                return building.gameObject;
            }
        }

        static void ClearScene()
        {
            var findall = Resources.FindObjectsOfTypeAll<Building>();
            foreach (var b in findall)
            {
                if (b.gameObject.scene.name == SceneManagerHelper.ActiveSceneName)
                    GameObject.Destroy(b.gameObject);
            }

            //cleanup loot we have already parsed
            var loot = Resources.FindObjectsOfTypeAll<Item>().Where(it => IsValidLoot(it));
            foreach (var item in loot)
                GameObject.Destroy(item.gameObject);

            //cleanup merchants
            var merchants = Resources.FindObjectsOfTypeAll<Merchant>();
            foreach (var merchant in merchants)
                GameObject.Destroy(merchant.gameObject);
        }

        #region PARSE ALL LOOT FUNCTION
        // Parse Loot
        public static void ParseAllLoot()
        {
            var allitems = Resources.FindObjectsOfTypeAll(typeof(Item)) as Item[];

            foreach (Item item in allitems.Where(x => IsValidLoot(x)))
            {
                var summary = ListManager.SceneSummaries[ListManager.GetSceneSummaryKey(item.transform.position)];

                if (item is SelfFilledItemContainer)
                {
                    if (item is TreasureChest)
                    {
                        var lootContainer = DM_LootContainer.ParseLootContainer(item as TreasureChest);
                        AddQuantity(lootContainer.Name, summary.Loot_Containers);

                        ListManager.AddContainerSummary(
                            lootContainer.Name, 
                            ListManager.GetSceneSummaryKey(item.transform.position), 
                            lootContainer.DropTables, 
                            Instance.GetCurrentLocation(item.transform.position)
                        );

                        if (!summary.UniqueContainerList.Contains(lootContainer.Name + "_" + lootContainer.UID))
                        {
                            summary.UniqueContainerList.Add(lootContainer.Name + "_" + lootContainer.UID);
                        }
                    }
                    else if (item is Gatherable)
                    {
                        var gatherableHolder = DM_Gatherable.ParseGatherable(item as Gatherable);
                        AddQuantity(gatherableHolder.Name, summary.Gatherables);

                        ListManager.AddContainerSummary(
                            gatherableHolder.Name, 
                            ListManager.GetSceneSummaryKey(item.transform.position), 
                            gatherableHolder.DropTables,
                            Instance.GetCurrentLocation(item.transform.position)
                        );
                    }
                    else
                    {
                        SL.LogWarning("[ParseLoot] Unsupported ItemContainer: " + item.Name + ", typeof: " + item.GetType());
                    }
                }
                else
                {
                    // item spawn
                    bool newHolder = true;
                    foreach (DM_ItemSpawn holder in summary.Item_Spawns)
                    {
                        if (holder.Item_ID == item.ItemID)
                        {
                            newHolder = false;
                            holder.Quantity++;
                            holder.positions.Add(item.transform.position);
                            break;
                        }
                    }
                    if (newHolder)
                    {
                        summary.Item_Spawns.Add(new DM_ItemSpawn
                        {
                            Name = item.Name?.Trim(),
                            Item_ID = item.ItemID,
                            Quantity = 1,
                            positions = new List<Vector3>
                            {
                                item.transform.position
                            }
                        });
                    }

                    //AddItemSpawnSource(item.ItemID, item.Name, item.transform.position);
                }
            }
        }
        #endregion

        #region GLOBAL HELPERS
        // Helpers used globally
        public string GetCurrentRegion()
        {
            string region = "ERROR";
            foreach (KeyValuePair<string, List<string>> entry in SceneHelper.ScenesByRegion)
            {
                if (entry.Value.Contains(SceneHelper.SceneBuildNames[SceneManagerHelper.ActiveSceneName]))
                {
                    region = entry.Key;
                    break;
                }
            }
            if (region == "ERROR")
            {
                if (SceneManagerHelper.ActiveSceneName.ToLower().Contains("cherso"))
                    region = "Chersonese";
                if (SceneManagerHelper.ActiveSceneName.ToLower().Contains("emercar"))
                    region = "Enmerkar Forest";
                if (SceneManagerHelper.ActiveSceneName.ToLower().Contains("abrassar"))
                    region = "Abrassar";
                if (SceneManagerHelper.ActiveSceneName.ToLower().Contains("hallowed"))
                    region = "Hallowed Marsh";
                if (SceneManagerHelper.ActiveSceneName.ToLower().Contains("antique"))
                    region = "Antique Plateau";
                if (SceneManagerHelper.ActiveSceneName.ToLower().Contains("caldera"))
                    region = "Caldera";
            }
            return region;
        }

        public string GetCurrentLocation(Vector3 _pos)
        {
            if (!SceneManagerHelper.ActiveSceneName.ToLower().Contains("dungeonssmall"))
            {
                return SceneHelper.SceneBuildNames[SceneManagerHelper.ActiveSceneName];
            }
            else
            {
                string closestRegion = "";
                float lowest = float.MaxValue;

                Dictionary<string, Vector3> dict = null;
                switch (GetCurrentRegion())
                {
                    case "Chersonese":
                        dict = SceneHelper.ChersoneseDungeons; break;
                    case "Enmerkar Forest":
                        dict = SceneHelper.EnmerkarDungeons; break;
                    case "Hallowed Marsh":
                        dict = SceneHelper.MarshDungeons; break;
                    case "Abrassar":
                        dict = SceneHelper.AbrassarDungeons; break;
                    case "Antique Plateau":
                        dict = SceneHelper.AntiqueDungeons; break;
                    case "Caldera":
                        dict = SceneHelper.CalderaDungeons; break;
                    default: break;
                }

                if (dict != null)
                {
                    foreach (KeyValuePair<string, Vector3> entry in dict)
                    {
                        if (Vector3.Distance(_pos, entry.Value) < lowest)
                        {
                            lowest = Vector3.Distance(_pos, entry.Value);
                            closestRegion = entry.Key;
                        }
                    }

                    return closestRegion;
                }
                else
                {
                    SL.LogWarning("Could not get region!");
                    return SceneManagerHelper.ActiveSceneName;
                }
            }
        }
        #endregion

        #region SMALL HELPERS
        //// Small helper functions
        //private static void AddItemSpawnSource(int item_ID, string item_Name, Vector3 pos)
        //{
        //    if (ListManager.ItemLootSources.ContainsKey(item_ID.ToString()))
        //    {
        //        ListManager.ItemLootSources[item_ID.ToString()].Spawn_Sources.Add(ListManager.GetSceneSummaryKey(pos));
        //    }
        //    else
        //    {
        //        ListManager.ItemLootSources.Add(item_ID.ToString(), new ItemSource
        //        {
        //            ItemID = item_ID,
        //            ItemName = item_Name,
        //            Container_Sources = new List<string>(),
        //            Spawn_Sources = new List<string>
        //            {
        //                ListManager.GetSceneSummaryKey(pos)
        //            }
        //        });
        //    }
        //}

        private static void AddQuantity(string name, List<QuantityHolder> list)
        {
            bool newEntry = true;
            foreach (QuantityHolder holder in list)
            {
                if (holder.Name == name)
                {
                    holder.Quantity++;
                    newEntry = false;
                    break;
                }
            }
            if (newEntry)
            {
                list.Add(new QuantityHolder
                {
                    Name = name,
                    Quantity = 1
                });
            }
        }

        public static bool IsValidLoot(Item item)
        {
            if (item.gameObject.scene == null || item.UID == null || item.UID == UID.Empty || ItemManager.Instance.GetItem(item.UID) == null)
            {
                return false;
            }

            if (item.ParentContainer == null && item.OwnerCharacter == null && item.IsInWorld && (item.IsPickable || item is SelfFilledItemContainer || item.IsDeployable))
            {
                return true;
            }

            return false;
        }

        private void ForceObjectActive(GameObject obj)
        {
            obj.SetActive(true);
            if (obj.transform.parent != null)
            {
                ForceObjectActive(obj.transform.parent.gameObject);
            }
        }

        public void DisableCanvases()
        {
            var canvases = Resources.FindObjectsOfTypeAll(typeof(NodeCanvas.BehaviourTrees.BehaviourTreeOwner));

            foreach (NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree in canvases)
            {
                tree.gameObject.SetActive(false);
            }
        }
        #endregion

        #region HOOKS

        [HarmonyPatch(typeof(AICEnemyDetection), "Update")]
        public class AICEnemyDetection_Update
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (Instance.m_parsing)
                {
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(AIESwitchState), "SwitchState")]
        public class AIESwitchState_Update
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (Instance.m_parsing)
                {
                    return false;
                }

                return true;
            }
        }
        #endregion
    }
}
