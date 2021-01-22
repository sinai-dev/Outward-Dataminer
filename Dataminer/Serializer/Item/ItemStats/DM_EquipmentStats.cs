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
        public float Impact_Bonus;

        public float Stamina_Use_Penalty;
        public float Mana_Use_Modifier;
        public float Mana_Regen;
        public float Movement_Penalty;

        public float Pouch_Bonus;
        public float Heat_Protection;
        public float Cold_Protection;
        
        public float Corruption_Protection;
        public float Cooldown_Reduction;
        public float Hunger_Affect;
        public float Thirst_Affect;
        public float Fatigue_Affect;

        public float BarrierProtection;
        public float GlobalStatusEffectResistance;
        public float StaminaRegenModifier;

        public override void SerializeStats(ItemStats stats, DM_ItemStats holder)
        {
            base.SerializeStats(stats, holder);

            try
            {
                var item = stats.GetComponent<Item>();

                var eStats = stats as EquipmentStats;
                Cooldown_Reduction = eStats.CooldownReduction;                
                Hunger_Affect = eStats.HungerModifier;                
                Thirst_Affect = eStats.ThirstModifier;                
                Fatigue_Affect = eStats.SleepModifier;

                Impact_Resistance = eStats.ImpactResistance;
                Damage_Protection = eStats.GetDamageProtection(DamageType.Types.Physical);
                Stamina_Use_Penalty = eStats.StaminaUsePenalty;
                Mana_Use_Modifier = (float)At.GetField(eStats, "m_manaUseModifier");
                Mana_Regen = eStats.ManaRegenBonus;
                Movement_Penalty = eStats.MovementPenalty;
                Pouch_Bonus = eStats.PouchCapacityBonus;
                Heat_Protection = eStats.HeatProtection;
                Cold_Protection = eStats.ColdProtection;
                Corruption_Protection = eStats.CorruptionResistance;

                Damage_Bonus = At.GetField(eStats, "m_damageAttack") as float[];
                Damage_Resistance = At.GetField(eStats, "m_damageResistance") as float[];
                Impact_Bonus = eStats.ImpactModifier;

                BarrierProtection = eStats.BarrierProtection;
                GlobalStatusEffectResistance = (float)At.GetField(eStats, "m_baseStatusEffectResistance");
                StaminaRegenModifier = eStats.StaminaRegenModifier;
            }
            catch (Exception e)
            {
                SL.Log("Exception getting stats of " + stats.name + "\r\n" + e.Message + "\r\n" + e.StackTrace);
            }
        }
    }
}
