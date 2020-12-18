using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using Rewired.Data;
using SideLoader;

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

        public float OverrideSellModifier;

        // for Perishable ItemExtension
        public string PerishTime;

        public List<string> Tags = new List<string>();
        public DM_ItemStats StatsHolder;
        public List<DM_EffectTransform> EffectTransforms = new List<DM_EffectTransform>();   

        public static void ParseAllItems()
        {
            if (References.RPM_ITEM_PREFABS is Dictionary<string, Item> ItemPrefabs)
            {
                foreach (Item item in ItemPrefabs.Values)
                {
                    try
                    {
                        var safename = Serializer.SafeName(item.Name);

                        //if (item.ItemIcon && !item.HasDefaultIcon)
                        //{
                        //    SideLoader.CustomTextures.SaveIconAsPNG(item.ItemIcon, "ItemIcons", safename);
                        //}

                        //SL.Log("Parsing " + item.Name + ", typeof: " + item.GetType());

                        // Parse the item. This will recursively dive.
                        var itemHolder = ParseItemToTemplate(item);

                        ListManager.Items.Add(item.ItemID.ToString(), itemHolder);

                        string dir = GetFullSaveDir(item, itemHolder);
                        Serializer.SaveToXml(dir, safename + " (" + item.gameObject.name + ")", itemHolder);
                    }
                    catch (Exception e) 
                    {
                        SL.LogWarning(e.ToString());
                    }
                }
            }
        }

        public static DM_Item ParseItemToTemplate(Item item)
        {
            if (item == null)
                return null;
            //SL.Log("Parsing item to template: " + item.Name);

            var type = Serializer.GetBestDMType(item.GetType());

            DM_Item holder;
            try
            {
                holder = (DM_Item)Activator.CreateInstance(type);
            }
            catch (InvalidCastException)
            {
                if (type == null)
                    SL.LogWarning("type is null");
                else
                    SL.LogWarning("Exception casting '" + type.FullName + "' to DM_Item?");

                return null;
            }

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

            holder.OverrideSellModifier = (float)At.GetField(item, "m_overrideSellModifier");

            if (item.GetComponent<Perishable>() is Perishable perish)
            {
                float perishRate = perish.DepletionRate * 0.03333333f;
                float perishModifier = 1 / perishRate;

                var remainingTicks = item.MaxDurability * perishModifier; // each tick is 2 in-game minutes (~5 seconds irl)
                var minutes = remainingTicks * 2;
                TimeSpan t = TimeSpan.FromMinutes(minutes);

                holder.PerishTime = $"{t.Days} Days, {t.Hours} Hours, {t.Minutes} Minutes, {t.Seconds} Seconds";
            }

            holder.CastType = (Character.SpellCastType)At.GetField(item, "m_activateEffectAnimType");

            if (item.GetComponent<ItemStats>() is ItemStats stats)
            {
                holder.StatsHolder = DM_ItemStats.ParseItemStats(stats);
            }

            if (item.Tags != null)
            {
                foreach (Tag tag in item.Tags)
                {
                    holder.Tags.Add(tag.TagName);

                    ListManager.AddTagSource(tag, Serializer.SafeName(item.Name));
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
