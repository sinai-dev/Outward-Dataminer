﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using SideLoader;
using NodeCanvas.Framework;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.DialogueTrees;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Enemy : IEquatable<DM_Enemy>
    {
        public string Name;
        public string GameObject_Name; // with UID removed
        public int Unique_ID;

        public List<string> Locations_Found = new List<string>();

        public float Max_Health;
        public float Health_Regen_Per_Second;
        public float Impact_Resistance;
        public float[] Protection;
        public float[] Damage_Resistances;
        public float[] Damage_Bonuses;
        public float Collider_Radius;

        public bool NullifyResistances;
        public float GlobalStatusRes;
        public float BarrierStat;

        public List<Damages> True_WeaponDamage = new List<Damages>();
        public float Weapon_Impact;

        public List<string> Status_Immunities = new List<string>();

        public List<string> Starting_Equipment = new List<string>();
        public List<string> Skill_Knowledge = new List<string>();

        public string Faction;
        public bool Allied_To_Same_Faction;

        public List<DropTableEntry> Guaranteed_Drops = new List<DropTableEntry>();
        public List<string> DropTable_Names = new List<string>();

        public static void ParseAllEnemies()
        {
            var potentialEnemies = GetPotentialEnemyUIDs();

            var enemies = CharacterManager.Instance.Characters.Values
                    .Where(x => x.IsAI
                        && (x.Faction != Character.Factions.Player || potentialEnemies.Contains(x.UID))
                        && x.Stats);

            foreach (Character enemy in enemies)
            {
                var enemyHolder = ParseEnemy(enemy);

                if (enemyHolder != null)
                {
                    var summary = ListManager.SceneSummaries[ListManager.GetSceneSummaryKey(enemy.transform.position)];

                    // add to scene summary
                    string saveName = enemyHolder.Name + " (" + enemyHolder.Unique_ID + ")";
                    bool found = false;
                    foreach (QuantityHolder holder in summary.Enemies)
                    {
                        if (holder.Name == saveName)
                        {
                            // list contains this unique ID. add to.
                            holder.Quantity++;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        summary.Enemies.Add(new QuantityHolder
                        {
                            Name = saveName,
                            Quantity = 1
                        });
                    }
                }
            }
        }

        private static HashSet<string> GetPotentialEnemyUIDs()
        {
            var ret = new HashSet<string>();

            try
            {
                List<Graph> graphs = new List<Graph>();

                graphs.AddRange(Resources.FindObjectsOfTypeAll<BehaviourTreeOwner>()
                    .Where(it => it.graph != null && it.graph.allNodes?.Count > 0)
                    .Select(it => it.graph));
                graphs.AddRange(Resources.FindObjectsOfTypeAll<DialogueTreeController>()
                    .Where(it => it.graph != null && it.graph.allNodes?.Count > 0)
                    .Select(it => it.graph));

                foreach (var graph in graphs)
                {
                    try
                    {
                        var actions = new List<ActionTask>();

                        foreach (var node in graph.allNodes)
                        {
                            try
                            {
                                if (node is NodeCanvas.BehaviourTrees.ActionNode actionNode)
                                {
                                    if (actionNode.action is ActionList actionList && actionList.actions?.Count > 0)
                                        actions.AddRange(actionList.actions);
                                    else if (actionNode.action != null)
                                        actions.Add(actionNode.action);
                                }
                                else if (node is NodeCanvas.DialogueTrees.ActionNode dialogNode)
                                {
                                    if (dialogNode.action is ActionList actionList && actionList.actions?.Count > 0)
                                        actions.AddRange(actionList.actions);
                                    else if (dialogNode.action != null)
                                        actions.Add(dialogNode.action);
                                }
                            }
                            catch (Exception e)
                            {
                                SL.LogWarning("Exception parsing node " + node.name);
                                SL.LogInnerException(e);
                            }
                        }

                        foreach (var action in actions)
                        {
                            try
                            {
                                if (action is ChangeCharacterFaction changeCharFaction)
                                {
                                    ret.Add((string)At.GetField(changeCharFaction, "m_targetUID"));
                                }
                                else if (action is ChangeChildCharactersFaction changeChildrenFactions)
                                {
                                    if (changeChildrenFactions.Parent?.value)
                                        foreach (var character in changeChildrenFactions.Parent.value.GetComponentsInChildren<Character>(true))
                                            ret.Add(character.UID);
                                }
                            }
                            catch (Exception e)
                            {
                                SL.LogWarning("Exception checking action " + action.name);
                                SL.LogInnerException(e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        SL.LogWarning("Exception parsing graph " + graph.name);
                        SL.LogInnerException(e);
                    }
                }
            }
            catch (Exception e)
            {
                SL.LogWarning("Exception getting potential enemy UIDs");
                SL.LogInnerException(e);
            }

            SL.LogWarning("Found " + ret.Count + " potential enemies who are marked as Players");

            return ret;
        }

        public static DM_Enemy ParseEnemy(Character character)
        {
            SL.Log("parsing enemy " + character.Name + " (" + character.gameObject.name + ")");
            var pos = character.transform.position;

            var enemyHolder = new DM_Enemy
            {
                Name = character.Name,
                Unique_ID = 1,
                Max_Health = character.Stats.MaxHealth,
                Faction = character.Faction.ToString(),
                Allied_To_Same_Faction = character.TargetingSystem.AlliedToSameFaction,
                Collider_Radius = character.CharacterController.radius
            };

            if (character.Stats)
            {
                enemyHolder.NullifyResistances = character.Stats.NullifyResistances;
                enemyHolder.GlobalStatusRes = character.Stats.GlobalStatusRes;
                enemyHolder.BarrierStat = ((Stat)At.GetField(character.Stats, "m_barrierStat"))?.BaseValue ?? -1f;
            }

            FixName(enemyHolder, character);

            // add to Locations (first location)
            string location = ListManager.GetSceneSummaryKey(pos);
            enemyHolder.Locations_Found.Add(location);

            // get Status Immunities
            GetStatusImmunities(enemyHolder, character);

            // get Drops (and dropper info)
            bool dropWeapon = false;
            bool dropPouch = false;
            GetDrops(enemyHolder, character, ref dropWeapon, ref dropPouch);

            // get stats
            GetStats(enemyHolder, character);

            // get Starting Equipment
            try
            {
                GetEquipment(enemyHolder, character, dropWeapon, dropPouch);
            }
            catch (Exception e)
            {
                SL.LogWarning("Exception parsing startingequipment of " + character.Name + ", messsage: " + e.Message);
            }

            // round stats nicely
            for (int i = 0; i < 6; i++)
            {
                enemyHolder.Damage_Bonuses[i] = (float)Math.Round(enemyHolder.Damage_Bonuses[i], 2);
                enemyHolder.Damage_Resistances[i] = (float)Math.Round(enemyHolder.Damage_Resistances[i], 2);
                enemyHolder.Protection[i] = (float)Math.Round(enemyHolder.Protection[i], 2);
            }

            // compare to existing enemies, see if we should save as new unique
            CheckUnique(enemyHolder, location);

            return enemyHolder;
        }

        // Fix Name
        private static void FixName(DM_Enemy enemyHolder, Character character)
        {
            // fix a few enemy names
            switch (enemyHolder.Name)
            {
                case "???":
                    enemyHolder.Name = "Concealed Knight"; break;
                default: break;
            }
            if (enemyHolder.Name.ToLower().Contains("bandit")
                && SceneManager.Instance.GetCurrentLocation(character.CenterPosition).ToLower().Contains("vendavel"))
            {
                enemyHolder.Name = "Vendavel " + enemyHolder.Name;
            }

            // regex and fix the gameobject name, remove the UID and the (count)
            string goName = character.gameObject.name;
            goName = goName.Substring(0, goName.Length - (character.UID.ToString().Length + 1));
            goName = Regex.Replace(goName, @"\s*[(][\d][)]$", "");
            enemyHolder.GameObject_Name = goName;
        }

        // Status Effect Immunity
        private static void GetStatusImmunities(DM_Enemy enemyHolder, Character character)
        {
            // Immunities
            foreach (TagSourceSelector tagSelector in At.GetField(character.Stats, "m_statusEffectsNaturalImmunity") as TagSourceSelector[])
            {
                enemyHolder.Status_Immunities.Add(tagSelector.Tag.TagName);
            }
            foreach (KeyValuePair<Tag, List<string>> entry in At.GetField(character.Stats, "m_statusEffectsImmunity") as Dictionary<Tag, List<string>>)
            {
                if (entry.Value.Count > 0)
                {
                    enemyHolder.Status_Immunities.Add(entry.Key.TagName);
                }
            }
        }

        // DropTables
        private static void GetDrops(DM_Enemy enemyHolder, Character character, ref bool dropWeapon, ref bool dropPouch)
        {
            if (character.GetComponent<LootableOnDeath>() is LootableOnDeath lootableOnDeath)
            {
                try
                {
                    dropWeapon = lootableOnDeath.DropWeapons;
                    dropPouch = lootableOnDeath.EnabledPouch;

                    if (At.GetField(lootableOnDeath, "m_lootDroppers") is Dropable[] m_lootDroppers)
                    {
                        foreach (Dropable dropper in m_lootDroppers)
                        {
                            At.Invoke(dropper, "InitReferences");
                            var dropTableHolder = DM_DropTable.ParseDropable(dropper);
                            enemyHolder.DropTable_Names.Add(dropTableHolder.Name);
                        }
                    }
                }
                catch (Exception e)
                {
                    SL.LogError("Exception parsing enemy droptable: " + e.Message);
                }
            }
        }

        // stats
        private static void GetStats(DM_Enemy enemyHolder, Character character)
        {
            if (character.Stats == null)
            {
                SL.LogError("Character stats is null!");
                return;
            }

            if (At.GetField(character.Stats, "m_healthRegen") is Stat healthRegen)
            {
                enemyHolder.Health_Regen_Per_Second = healthRegen.BaseValue;
            }

            // impact resist
            enemyHolder.Impact_Resistance = character.Stats.ImpactResistanceStat.BaseValue;

            // damage stats
            List<float> damageBonuses = new List<float>();
            List<float> damageRes = new List<float>();
            List<float> damageProt = new List<float>();

            var orig_DmgBonus = At.GetField(character.Stats, "m_damageTypesModifier") as Stat[];
            var orig_Res = At.GetField(character.Stats, "m_damageResistance") as Stat[];
            var orig_Prot = At.GetField(character.Stats, "m_damageProtection") as Stat[];

            for (int i = 0; i < 6; i++)
            {
                // damage bonuses
                damageBonuses.Add((orig_DmgBonus[i].BaseValue - 1) * 100f);

                // damage res
                damageRes.Add(orig_Res[i].BaseValue);

                // protection
                damageProt.Add(orig_Prot[i].BaseValue);
            }

            enemyHolder.Damage_Bonuses = damageBonuses.ToArray();
            enemyHolder.Damage_Resistances = damageRes.ToArray();
            enemyHolder.Protection = damageProt.ToArray();
        }

        // Starting Equipment (and EquipmentStats)
        private static void GetEquipment(DM_Enemy enemyHolder, Character character, bool dropWeapon, bool dropPouch)
        {
            if (character.GetComponent<StartingEquipment>() is StartingEquipment startingEquipment)
            {
                if (startingEquipment.StartingSkills != null)
                {
                    foreach (Skill skill in startingEquipment.StartingSkills.Where(x => x != null))
                    {
                        enemyHolder.Skill_Knowledge.Add((skill.Name ?? skill.name) + " (" + skill.ItemID + ")");
                    }
                }
            }

            if (character.Inventory.Pouch != null && dropPouch)
            {
                foreach (Item item in character.Inventory.Pouch.GetContainedItems())
                {
                    if (item.IsPickable)
                    {
                        int qty = 1;
                        if (item.IsStackable)
                        {
                            qty = item.GetComponent<MultipleUsage>().RemainingAmount;
                        }

                        AddGuaranteedDrop(enemyHolder, item.ItemID, item.Name, qty, (qty == 1 ? -1 : qty), true);
                    }
                }
            }
            
            if (character.Inventory.Equipment != null)
            {
                foreach (Equipment equipment in character.Inventory.Equipment.GetComponentsInChildren<Equipment>())
                {
                    enemyHolder.Starting_Equipment.Add(equipment.Name + " (" + equipment.ItemID + ")");

                    var stats = equipment.GetComponent<EquipmentStats>() ?? ResourcesPrefabManager.Instance.GetItemPrefab(equipment.ItemID).Stats as EquipmentStats;

                    if (stats != null)
                    {
                        if (!(stats is WeaponStats))
                        {
                            // weaponstats cant give impact resistance (only used for blocking)
                            enemyHolder.Impact_Resistance += stats.ImpactResistance;
                        }

                        var damageBonus = At.GetField(stats, "m_damageAttack") as float[];
                        var damageRes = At.GetField(stats, "m_damageResistance") as float[];
                        var damageProt = At.GetField(stats, "m_damageProtection") as float[];

                        for (int i = 0; i < 6; i++)
                        {
                            enemyHolder.Damage_Bonuses[i] += damageBonus[i];
                            enemyHolder.Damage_Resistances[i] += damageRes[i];
                            enemyHolder.Protection[i] += damageProt[i];
                        }
                    }

                    // add weapon drops to guaranteed drops
                    if (equipment is Weapon && dropWeapon && equipment.IsPickable)
                    {
                        if (equipment is Ammunition)
                        {
                            int count = (int)At.GetField(equipment as Ammunition, "m_currentCapacity");
                            AddGuaranteedDrop(enemyHolder, equipment.ItemID, equipment.Name, count, -1, true);
                        }
                        else
                        {
                            AddGuaranteedDrop(enemyHolder, equipment.ItemID, equipment.Name, 1);
                        }
                    }
                }

                // fix weapon damage after parsing all equipment
                Weapon charWeapon = null;
                if (character.CurrentWeapon != null)
                {
                    charWeapon = character.CurrentWeapon;
                }
                else
                {
                    foreach (Weapon weapon in character.Inventory.Equipment.GetComponentsInChildren<Weapon>())
                    {
                        if (weapon.Type != Weapon.WeaponType.Shield && !(weapon is Ammunition))
                        {
                            charWeapon = weapon;
                            break;
                        }
                    }
                }
                if (charWeapon != null)
                {
                    SetWeaponDamage(enemyHolder, charWeapon);
                }
            }
        }

        // Add to guaranteed drops
        private static void AddGuaranteedDrop(DM_Enemy holder, int id, string name, int qty, int maxQty = -1, bool cumulative = false)
        {
            foreach (DropTableEntry entry in holder.Guaranteed_Drops)
            {
                if (entry.Item_ID == id)
                {
                    if (cumulative)
                    {
                        entry.Min_Quantity += qty;
                        entry.Max_Quantity += maxQty == -1 ? qty : maxQty;
                    }
                    return;
                }
            }

            holder.Guaranteed_Drops.Add(new DropTableEntry
            {
                Item_ID = id,
                Item_Name = name,
                Min_Quantity = qty,
                Max_Quantity = maxQty == -1 ? qty : maxQty
            });
        }

        // Weapon Damages
        private static void SetWeaponDamage(DM_Enemy holder, Weapon weapon)
        {
            WeaponStats stats = weapon.Stats ?? ResourcesPrefabManager.Instance.GetItemPrefab(weapon.ItemID).Stats as WeaponStats;

            if (stats != null)
            {
                holder.Weapon_Impact = stats.Impact;

                var list = stats.BaseDamage.Clone();
                for (int i = 0; i < 6; i++)
                {
                    float multi = holder.Damage_Bonuses[i];
                    if (list[(DamageType.Types)i] != null)
                    {
                        list[(DamageType.Types)i].Damage *= 1 + (0.01f * multi);
                        list[(DamageType.Types)i].Damage = (float)Math.Round(list[(DamageType.Types)i].Damage, 2);
                    }
                }
                holder.True_WeaponDamage = Damages.ParseDamageList(list);
            }
            else
            {
                SL.LogWarning("Null stats for " + weapon.Name);
            }
        }

        // Check if unique
        private static void CheckUnique(DM_Enemy enemyHolder, string location)
        {
            bool newSave = true;
            if (ListManager.EnemyManifest.ContainsKey(enemyHolder.Name))
            {
                bool newVariant = true;
                int count = 1;
                foreach (DM_Enemy existingHolder in ListManager.EnemyManifest[enemyHolder.Name])
                {
                    if (enemyHolder.Equals(existingHolder))
                    {
                        SL.Log("Enemy was a copy of ID " + count);
                        enemyHolder.Unique_ID = existingHolder.Unique_ID;

                        newVariant = false;
                        newSave = false;
                        if (!existingHolder.Locations_Found.Contains(location))
                        {
                            existingHolder.Locations_Found.Add(location);
                            SaveEnemy(existingHolder);
                        }
                        break;
                    }
                    count++;
                }
                if (newVariant)
                {
                    enemyHolder.Unique_ID = count;
                    ListManager.EnemyManifest[enemyHolder.Name].Add(enemyHolder);
                }
            }
            else // new Unique
            {
                ListManager.EnemyManifest.Add(enemyHolder.Name, new List<DM_Enemy> { enemyHolder });
            }

            if (newSave)
            {
                SaveEnemy(enemyHolder);
            }
        }

        // Actual save
        private static void SaveEnemy(DM_Enemy holder)
        {
            SL.LogWarning(string.Format("Saving enemy '{0}' (unique ID: {1})", holder.Name, holder.Unique_ID));

            var dir = Serializer.Folders.Enemies;
            string saveName = holder.Name + " (" + holder.Unique_ID + ")";

            // overwrite all enemies, to update Locations_Found list automatically.
            string path = dir + "/" + saveName + ".xml";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Serializer.SaveToXml(dir, saveName, holder);
        }

        // Custom Equals comparer
        public bool Equals(DM_Enemy other)
        {
            bool equal = this.Max_Health == other.Max_Health
                && this.Health_Regen_Per_Second == other.Health_Regen_Per_Second
                && this.Impact_Resistance == other.Impact_Resistance
                && this.Guaranteed_Drops.Count == other.Guaranteed_Drops.Count
                && this.Starting_Equipment.Count == other.Starting_Equipment.Count
                && this.DropTable_Names.Count == other.DropTable_Names.Count
                && this.Status_Immunities.Count == other.Status_Immunities.Count
                && this.Skill_Knowledge.Count == other.Skill_Knowledge.Count;

            if (equal)
            {
                // check guaranteed drops
                for (int i = 0; i < this.Guaranteed_Drops.Count; i++)
                {
                    if (this.Guaranteed_Drops[i].Item_ID != other.Guaranteed_Drops[i].Item_ID
                        || this.Guaranteed_Drops[i].Min_Quantity != other.Guaranteed_Drops[i].Min_Quantity
                        || this.Guaranteed_Drops[i].Max_Quantity != other.Guaranteed_Drops[i].Max_Quantity)
                    {
                        return false;
                    }
                }

                // check drop table names
                for (int i = 0; i < this.DropTable_Names.Count; i++)
                {
                    if (this.DropTable_Names[i] != other.DropTable_Names[i])
                    {
                        return false;
                    }
                }

                // check status immunities
                for (int i = 0; i < this.Status_Immunities.Count; i++)
                {
                    if (this.Status_Immunities[i] != other.Status_Immunities[i])
                    {
                        return false;
                    }
                }

                // check skills
                for (int i = 0; i < this.Skill_Knowledge.Count; i++)
                {
                    if (this.Skill_Knowledge[i] != other.Skill_Knowledge[i])
                    {
                        return false;
                    }
                }

                // check damage bonus and resistances
                for (int i = 0; i < 6; i++)
                {
                    if (this.Damage_Resistances[i] != other.Damage_Resistances[i] 
                        || this.Damage_Bonuses[i] != other.Damage_Bonuses[i]
                        || this.Protection[i] != other.Protection[i])
                    {
                        return false;
                    }
                }

                // check equipment
                for (int i = 0; i < this.Starting_Equipment.Count; i++)
                {
                    if (this.Starting_Equipment[i] != other.Starting_Equipment[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
