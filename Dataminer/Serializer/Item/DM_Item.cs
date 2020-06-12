using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using Rewired.Data;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Item
    {
        public string gameObjectName;
        public string saveDir;

        public int ItemID;
        public string Name;
        public string Description;

        public int LegacyItemID;
        public bool IsPickable;
        public bool IsUsable;
        public int QtyRemovedOnUse;
        public bool GroupItemInDisplay;
        public bool HasPhysicsWhenWorld;
        public bool RepairedInRest;
        public Item.BehaviorOnNoDurabilityType BehaviorOnNoDurability;
        public Character.SpellCastType CastType;
        public Character.SpellCastModifier CastModifier;
        public bool CastLocomotionEnabled;
        public float MobileCastMovementMult;
        public int CastSheatheRequired;

        // for Perishable ItemExtension
        public string PerishTime;

        public List<string> Tags = new List<string>();
        public DM_ItemStats StatsHolder;
        public List<DM_EffectTransform> EffectTransforms = new List<DM_EffectTransform>();   

        public static void ParseAllItems()
        {
            if (At.GetValue(typeof(ResourcesPrefabManager), null, "ITEM_PREFABS") is Dictionary<string, Item> ItemPrefabs)
            {
                foreach (Item item in ItemPrefabs.Values)
                {
                    try
                    {
                        //Debug.Log("Parsing " + item.Name + ", typeof: " + item.GetType());

                        // Parse the item. This will recursively dive.
                        var itemHolder = ParseItemToTemplate(item);

                        ListManager.Items.Add(item.ItemID.ToString(), itemHolder);

                        // Folder and Save Name
                        string dir = GetFullSaveDir(item, itemHolder);
                        string saveName = item.Name + " (" + item.gameObject.name + ")";

                        Serializer.SaveToXml(dir, saveName, itemHolder);
                    }
                    catch { }
                }
            }
        }

        public static DM_Item ParseItemToTemplate(Item item)
        {
            //Debug.Log("Parsing item to template: " + item.Name);

            var type = Serializer.GetBestDMType(item.GetType());

            var holder = (DM_Item)Activator.CreateInstance(type);

            holder.SerializeItem(item, holder);

            return holder;
        }

        public virtual void SerializeItem(Item item, DM_Item holder)
        {
            holder.gameObjectName = item.gameObject.name;

            holder.ItemID = item.ItemID;
            holder.Name = item.Name;
            holder.Description = item.Description;
            holder.LegacyItemID = item.LegacyItemID;
            holder.CastLocomotionEnabled = item.CastLocomotionEnabled;
            holder.CastModifier = item.CastModifier;
            holder.CastSheatheRequired = item.CastSheathRequired;
            holder.GroupItemInDisplay = item.GroupItemInDisplay;
            holder.HasPhysicsWhenWorld = item.HasPhysicsWhenWorld;
            holder.IsPickable = item.IsPickable;
            holder.IsUsable = item.IsUsable;
            holder.QtyRemovedOnUse = item.QtyRemovedOnUse;
            holder.MobileCastMovementMult = item.MobileCastMovementMult;
            holder.RepairedInRest = item.RepairedInRest;
            holder.BehaviorOnNoDurability = item.BehaviorOnNoDurability;

            if (item.GetComponent<Perishable>() is Perishable perish)
            {
                float perishRate = perish.DepletionRate * 0.03333333f;
                float perishModifier = 1 / perishRate;

                var remainingTicks = item.MaxDurability * perishModifier; // est game time in seconds
                var minutes = remainingTicks * 2;
                TimeSpan t = TimeSpan.FromMinutes(minutes);

                holder.PerishTime = $"{t.Days} Days, {t.Hours} Hours, {t.Minutes} Minutes, {t.Seconds} Seconds";
            }

            holder.CastType = (Character.SpellCastType)At.GetValue(typeof(Item), item, "m_activateEffectAnimType");

            if (item.GetComponent<ItemStats>() is ItemStats stats)
            {
                holder.StatsHolder = DM_ItemStats.ParseItemStats(stats);
            }

            if (item.Tags != null)
            {
                foreach (Tag tag in item.Tags)
                {
                    holder.Tags.Add(tag.TagName);
                }
            }

            foreach (Transform child in item.transform)
            {
                var effectsChild = DM_EffectTransform.ParseTransform(child);

                if (effectsChild.ChildEffects.Count > 0 || effectsChild.Effects.Count > 0 || effectsChild.EffectConditions.Count > 0)
                {
                    holder.EffectTransforms.Add(effectsChild);
                }
            }
        }

        private static string GetFullSaveDir(Item item, DM_Item template)
        {
            string dir;

            if (item.GetType() != typeof(Item))
            {
                dir = GetFolderRecursive(item, "", item.GetType());
            }
            else
            {
                if (template.Tags.Contains("Consummable"))
                {
                    dir = "/Consumable";
                }
                else
                {
                    dir = "/_Unsorted";
                }
            }

            template.saveDir = dir;

            return Serializer.Folders.Items + dir;
        }

        private static string GetFolderRecursive(Item item, string currentDir, Type type)
        {
            currentDir = "/" + type + currentDir;
            var baseType = type.BaseType;
            if (baseType != null && baseType != typeof(Item))
            {
                currentDir = GetFolderRecursive(item, currentDir, baseType);
            }
            return currentDir;
        }
    }
}
