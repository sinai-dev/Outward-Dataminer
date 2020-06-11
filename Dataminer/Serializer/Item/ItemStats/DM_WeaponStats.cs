﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_WeaponStats : DM_EquipmentStats
    {
        public float AttackSpeed;
        public List<Damages> BaseDamage;
        public float Impact;
        public float StamCost;

        public bool AutoGenerateAttackData;
        public WeaponStats.AttackData[] Attacks;

        public override void SerializeStats(ItemStats stats, DM_ItemStats holder)
        {
            base.SerializeStats(stats, holder);

            var template = holder as DM_WeaponStats;

            var wStats = stats as WeaponStats;

            template.Attacks = wStats.Attacks;
            template.AttackSpeed = wStats.AttackSpeed;
            template.Impact = wStats.Impact;
            template.StamCost = wStats.StamCost;

            template.BaseDamage = Damages.ParseDamageList(wStats.BaseDamage);
        }

        public static WeaponStats.AttackData[] GetScaledAttackData(Weapon weapon)
        {
            var type = weapon.Type;
            var stats = weapon.GetComponent<WeaponStats>();
            var damage = stats.BaseDamage;
            var impact = stats.Impact;
            var stamCost = stats.StamCost;

            if (!WeaponBaseDataDict.ContainsKey(type))
            {
                return new WeaponStats.AttackData[]
                {
                    new WeaponStats.AttackData()
                    {
                        Damage = DamageListToFloatArray(damage, 1.0f),
                        Knockback = impact,
                        StamCost = stamCost
                    }
                };
            }

            var basedata = WeaponBaseDataDict[type];

            var list = new List<WeaponStats.AttackData>();

            for (int i = 0; i < 5; i++)
            {
                var attackdata = new WeaponStats.AttackData
                {
                    Damage = DamageListToFloatArray(damage, basedata.DamageMult[i]),
                    Knockback = impact * basedata.ImpactMult[i],
                    StamCost = stamCost * basedata.StamMult[i]
                };
                list.Add(attackdata);
            }

            return list.ToArray();
        }

        public static List<float> DamageListToFloatArray(DamageList list, float multiplier)
        {
            var floats = new List<float>();

            foreach (var type in list.List)
            {
                floats.Add(type.Damage * multiplier);
            }

            return floats;
        }

        public static Dictionary<Weapon.WeaponType, WeaponStatData> WeaponBaseDataDict = new Dictionary<Weapon.WeaponType, WeaponStatData>()
        {
            {
                Weapon.WeaponType.Sword_1H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.3f, 1.1f, 1.1f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.3f, 1.1f, 1.1f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.2f, 1.1f, 1.1f }
                }
            },
            {
                Weapon.WeaponType.Sword_2H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.5f, 1.1f, 1.1f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.5f, 1.1f, 1.1f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.3f, 1.1f, 1.1f } 
                }
            },
            {
                Weapon.WeaponType.Axe_1H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.2f, 1.2f, 1.2f } 
                }
            },
            {
                Weapon.WeaponType.Axe_2H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.2f, 1.1f, 1.1f } 
                }
            },
            {
                Weapon.WeaponType.Mace_1H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 2.5f, 1.3f, 1.3f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f } 
                }
            },
            {
                Weapon.WeaponType.Mace_2H,
                new WeaponStatData()
                {   //                         1     2     3      4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 0.75f, 1.4f, 1.4f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 2.0f,  1.4f, 1.4f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.2f,  1.2f, 1.2f } 
                }
            },
            {
                Weapon.WeaponType.Halberd_2H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.7f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.7f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.25f, 1.25f, 1.75f } 
                }
            },
            {
                Weapon.WeaponType.Spear_2H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.4f, 1.3f, 1.2f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.2f, 1.2f, 1.1f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.25f, 1.25f, 1.25f }
                }
            },
            {
                Weapon.WeaponType.FistW_2H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }
                }
            }
        };

        public class WeaponStatData
        {
            public float[] DamageMult;
            public float[] ImpactMult;
            public float[] StamMult;
        }
    }
}
