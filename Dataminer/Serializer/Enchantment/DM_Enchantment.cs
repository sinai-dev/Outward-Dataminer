using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Enchantment
    {
        public string Name;
        public string Description;

        public List<DM_Effect> Effects;

        public List<AdditionalDamage> AdditionalDamages;
        public List<StatModification> StatModifications;

        public List<Damages> DamageBonus;
        public List<Damages> DamageModifier;
        public List<Damages> ElementalResistances;

        public float HealthAbsorbRatio;
        public float ManaAbsorbRatio;
        public float StaminaAbsorbRatio;

        public float GlobalStatusResistance;

        public bool Indescructible;
        public float TrackDamageRatio;

        public static DM_Enchantment ParseEnchantment(Enchantment enchantment)
        {
            var template = new DM_Enchantment
            {
                Name = enchantment.Name,
                Description = enchantment.Description,

                HealthAbsorbRatio = enchantment.HealthAbsorbRatio,
                ManaAbsorbRatio = enchantment.ManaAbsorbRatio,
                StaminaAbsorbRatio = enchantment.StaminaAbsorbRatio,

                GlobalStatusResistance = enchantment.GlobalStatusResistance,

                Indescructible = enchantment.Indestructible,
                TrackDamageRatio = enchantment.TrackDamageRatio,
            };

            if (enchantment.Effects != null)
            {
                template.Effects = new List<DM_Effect>();
                foreach (var effect in enchantment.Effects)
                {
                    var effectTemplate = DM_Effect.ParseEffect(effect);

                    if (effectTemplate != null)
                    {
                        template.Effects.Add(effectTemplate);
                    }
                }
            }

            if (enchantment.AdditionalDamages != null)
            {
                template.AdditionalDamages = new List<AdditionalDamage>();
                foreach (var addDmg in enchantment.AdditionalDamages)
                {
                    template.AdditionalDamages.Add(new AdditionalDamage
                    {
                        BonusDamageType = addDmg.BonusDamageType,
                        ConversionRatio = addDmg.ConversionRatio,
                        SourceDamageType = addDmg.SourceDamageType
                    });
                }
            }            

            if (enchantment.StatModifications != null)
            {
                template.StatModifications = new List<StatModification>();
                foreach (var statMod in enchantment.StatModifications)
                {
                    template.StatModifications.Add(new StatModification 
                    {
                        Name = statMod.Name,
                        Type = statMod.Type,
                        Value = statMod.Value
                    });
                }
            }

            if (enchantment.DamageBonus != null)
            {
                template.DamageBonus = new List<Damages>();
                foreach (var dmgBonus in enchantment.DamageBonus.List)
                {
                    var dmg = Damages.ParseDamageType(dmgBonus);
                    template.DamageBonus.Add(dmg);
                }
            }

            if (enchantment.DamageModifier != null)
            {
                template.DamageModifier = new List<Damages>();
                foreach (var dmgBonus in enchantment.DamageModifier.List)
                {
                    var dmg = Damages.ParseDamageType(dmgBonus);
                    template.DamageModifier.Add(dmg);
                }
            }

            if (enchantment.ElementalResistances != null)
            {
                template.ElementalResistances = new List<Damages>();
                foreach (var dmgBonus in enchantment.ElementalResistances.List)
                {
                    var dmg = Damages.ParseDamageType(dmgBonus);
                    template.ElementalResistances.Add(dmg);
                }
            }

            return template;
        }

        public class AdditionalDamage
        {
            public DamageType.Types BonusDamageType;
            public float ConversionRatio;
            public DamageType.Types SourceDamageType;
        }

        public class StatModification
        {
            public Enchantment.Stat Name;
            public Enchantment.StatModification.BonusType Type;
            public float Value;
        }
    }
}
