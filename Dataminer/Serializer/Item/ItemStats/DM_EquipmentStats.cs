using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;
using UnityEngine;

namespace Dataminer
{
    public class DM_EquipmentStats : DM_ItemStats
    {
        public float[] Damage_Resistance = new float[9];
        public float Impact_Resistance;
        public float Damage_Protection;

        public float[] Damage_Bonus = new float[9];

        public float Stamina_Use_Penalty;
        public float Mana_Use_Modifier;
        public float Movement_Penalty;

        public float Pouch_Bonus;
        public float Heat_Protection;
        public float Cold_Protection;
        
        // DLC?
        public float Corruption_Protection;
        public float Cooldown_Reduction;
        public float Hunger_Affect;
        public float Thirst_Affect;
        public float Fatigue_Affect;


        public override void SerializeStats(ItemStats stats, DM_ItemStats holder)
        {
            base.SerializeStats(stats, holder);

            var equipmentStatsHolder = holder as DM_EquipmentStats;

            try
            {
                var item = stats.GetComponent<Item>();

                var eStats = stats as EquipmentStats;
                equipmentStatsHolder.Cooldown_Reduction = eStats.CooldownReduction;                
                equipmentStatsHolder.Hunger_Affect = eStats.HungerModifier;                
                equipmentStatsHolder.Thirst_Affect = eStats.ThirstModifier;                
                equipmentStatsHolder.Fatigue_Affect = eStats.SleepModifier;

                equipmentStatsHolder.Impact_Resistance = eStats.ImpactResistance;
                equipmentStatsHolder.Damage_Protection = eStats.GetDamageProtection(DamageType.Types.Physical);
                equipmentStatsHolder.Stamina_Use_Penalty = eStats.StaminaUsePenalty;
                equipmentStatsHolder.Mana_Use_Modifier = (float)At.GetField(stats as EquipmentStats, "m_manaUseModifier");
                equipmentStatsHolder.Movement_Penalty = eStats.MovementPenalty;
                equipmentStatsHolder.Pouch_Bonus = eStats.PouchCapacityBonus;
                equipmentStatsHolder.Heat_Protection = eStats.HeatProtection;
                equipmentStatsHolder.Cold_Protection = eStats.ColdProtection;
                equipmentStatsHolder.Corruption_Protection = eStats.CorruptionResistance;

                equipmentStatsHolder.Damage_Bonus = At.GetField(stats as EquipmentStats, "m_damageAttack") as float[];
                equipmentStatsHolder.Damage_Resistance = At.GetField(stats as EquipmentStats, "m_damageResistance") as float[];
            }
            catch (Exception e)
            {
                SL.Log("Exception getting stats of " + stats.name + "\r\n" + e.Message + "\r\n" + e.StackTrace);
            }
        }
    }
}
