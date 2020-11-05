using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BepInEx;
using HarmonyLib;

namespace Dataminer
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Dataminer : BaseUnityPlugin
    {
        public const string GUID = "com.sinai.dataminer";
        public const string NAME = "Dataminer";
        public const string VERSION = "2.0.0";

        public static bool NewDump { get; private set; } = false;

        [HarmonyPatch(typeof(AreaManager), nameof(AreaManager.IsAreaExpired))]
        public class AreaManager_IsAreaExpired
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        internal void Awake()
        {
            var obj = new GameObject("Dataminer");
            DontDestroyOnLoad(obj);
            obj.AddComponent<ListManager>();
            obj.AddComponent<SceneManager>();
            obj.AddComponent<AttackTimer>();

            var harmony = new Harmony(GUID);
            harmony.PatchAll();

            NewDump = Serializer.Folders.MakeFolders();
        }

        // calls Dump()
        [HarmonyPatch(typeof(ResourcesPrefabManager), "Load")]
        public class RPM_Load
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (NewDump)
                {
                    Dump();
                }
            }
        }

        // calls DM_Localization.SaveLocalization()
        [HarmonyPatch(typeof(LocalizationManager), "Load")]
        public class LocalizationManager_Load
        {
            [HarmonyPostfix]
            public static void Prefix(LocalizationManager __instance)
            {
                var localizationData = At.GetValue(typeof(LocalizationManager), __instance, "m_localizationData") as LocalizationReference;

                foreach (LocalizationReference.Localization loc in localizationData.Languages)
                {
                    if (loc.DefaultName == "English")
                    {
                        LocalizationHolder.SaveLocalization(loc);
                    }
                }
            }
        }

        public static void Dump()
        {
            Debug.LogWarning("[Dataminer] Starting full dump!");

            // setup tags
            for (int i = 1; i < 500; i++)
            {
                if (TagSourceManager.Instance.GetTag(i.ToString()) is Tag tag)
                {
                    ListManager.AddTagSource(tag, null);
                }
            }

            DM_Item.ParseAllItems();

            DM_StatusEffect.ParseAllEffects();

            DM_Recipe.ParseAllRecipes();

            DM_EnchantmentRecipe.ParseAllRecipes();

            ListManager.SaveEarlyLists();

            Debug.LogWarning("Finished prefab dumping.");
        }
    }
}
