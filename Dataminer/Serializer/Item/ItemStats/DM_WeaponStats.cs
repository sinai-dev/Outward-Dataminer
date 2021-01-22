using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;
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

            if (wStats.Attacks != null && wStats.Attacks.Length == 5)
            {
                var weapon = stats.GetComponent<Weapon>();
                CheckAttackDataMultipliers(weapon, wStats.Attacks);
            }

            template.Attacks = wStats.Attacks;
            template.AttackSpeed = wStats.AttackSpeed;
            template.Impact = wStats.Impact;
            template.StamCost = wStats.StamCost;

            template.BaseDamage = Damages.ParseDamageList(wStats.BaseDamage);
        }

        private void CheckAttackDataMultipliers(Weapon weapon, WeaponStats.AttackData[] datas)
        {
            if (weapon.IsAiStartingGear || !weapon.IsPickable || weapon.ItemID < 2000000)
            {
                return;
            }

            try
            {
                var autoScaled = GetScaledAttackData(weapon);

                for (int i = 0; i < datas.Length; i++)
                {
                    var data = datas[i];
                    var autoData = autoScaled[i];

                    if (!NearlyEquals(data.Damage[0], autoData.Damage[0]))
                    {
                        SL.Log($"[ScaleChecking] {weapon.Name} ({weapon.ItemID}), Data[{i}], Damage is not AutoScaled!");
                        for (int j = 0; j < data.Damage.Count; j++)
                        {
                            SL.Log("Damage[" + j + "], Real: " + data.Damage[j] + " (autoscaled: " + autoData.Damage[j] + ")");
                            var newRatio = data.Damage[j] / datas[0].Damage[j];
                            SL.Log("Actual ratio: " + newRatio);
                        }
                    }
                    if (!NearlyEquals(data.Knockback, autoData.Knockback))
                    {
                        SL.Log($"[ScaleChecking] {weapon.Name}, Data[{i}], Impact is not AutoScaled!");
                        SL.Log("Real: " + data.Knockback + ", autoscaled: " + autoData.Knockback);
                        var newRatio = data.Knockback / datas[0].Knockback;
                        SL.Log("Actual ratio: " + newRatio);
                    }
                    if (!NearlyEquals(data.StamCost, autoData.StamCost))
                    {
                        SL.Log($"[ScaleChecking] {weapon.Name}, Data[{i}], StaminaCost is not AutoScaled!");
                        SL.Log("Real: " + data.StamCost + ", autoscaled: " + autoData.StamCost);
                        var newRatio = data.StamCost / datas[0].StamCost;
                        SL.Log("Actual ratio: " + newRatio);
                    }
                }
            }
            catch { }
        }

        private bool NearlyEquals(float a, float b, float margin = 0.05f)
        {
            return Math.Abs(a - b) < margin;
        }

        #region Auto-scaling checking

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
                    DamageMult = new float[] { 1.0f, 1.0f, 1.495f, 1.265f, 1.265f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.3f, 1.1f, 1.1f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.2f, 1.1f, 1.1f }
                }
            },
            {
                Weapon.WeaponType.Sword_2H,
                new WeaponStatData()
                {   //                         1     2     3     4     5
                    DamageMult = new float[] { 1.0f, 1.0f, 1.5f, 1.265f, 1.265f },
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
                    StamMult   = new float[] { 1.0f, 1.0f, 1.375f, 1.375f, 1.35f }
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
                    DamageMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    ImpactMult = new float[] { 1.0f, 1.0f, 1.3f, 1.3f, 1.3f },
                    StamMult   = new float[] { 1.0f, 1.0f, 1.3f, 1.2f, 1.2f }
                }
            }
        };

        public class WeaponStatData
        {
            public float[] DamageMult;
            public float[] ImpactMult;
            public float[] StamMult;
        }
        #endregion
    }
}
