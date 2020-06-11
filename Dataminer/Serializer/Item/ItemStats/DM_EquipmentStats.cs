using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public float Corruption_Protection;

        public override void SerializeStats(ItemStats stats, DM_ItemStats holder)
        {
            base.SerializeStats(stats, holder);

            var equipmentStatsHolder = holder as DM_EquipmentStats;

            try
            {
                var eStats = stats as EquipmentStats;

                equipmentStatsHolder.Impact_Resistance = eStats.ImpactResistance;
                equipmentStatsHolder.Damage_Protection = eStats.GetDamageProtection(DamageType.Types.Physical);
                equipmentStatsHolder.Stamina_Use_Penalty = eStats.StaminaUsePenalty;
                equipmentStatsHolder.Mana_Use_Modifier = (float)At.GetValue(typeof(EquipmentStats), stats, "m_manaUseModifier");
                equipmentStatsHolder.Movement_Penalty = eStats.MovementPenalty;
                equipmentStatsHolder.Pouch_Bonus = eStats.PouchCapacityBonus;
                equipmentStatsHolder.Heat_Protection = eStats.HeatProtection;
                equipmentStatsHolder.Cold_Protection = eStats.ColdProtection;
                equipmentStatsHolder.Corruption_Protection = eStats.CorruptionResistance;

                equipmentStatsHolder.Damage_Bonus = At.GetValue(typeof(EquipmentStats), stats, "m_damageAttack") as float[];
                equipmentStatsHolder.Damage_Resistance = At.GetValue(typeof(EquipmentStats), stats, "m_damageResistance") as float[];
            }
            catch (Exception e)
            {
                Debug.Log("Exception getting stats of " + stats.name + "\r\n" + e.Message + "\r\n" + e.StackTrace);
            }
        }
    }
}
