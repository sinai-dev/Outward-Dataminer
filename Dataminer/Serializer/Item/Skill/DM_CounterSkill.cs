using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_CounterSkill : DM_MeleeSkill
    {
        public float BlockMult;
        public float DamageMult;
        public float KnockbackMult;

        public List<DamageType.Types> BlockDamageTypes;
        public List<DamageType.Types> CounterDamageTypes;

        public float? MaxRange;
        public bool? TurnTowardsDealer;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_CounterSkill;
            var skill = item as CounterSkill;

            template.BlockMult = skill.BlockMult;
            template.DamageMult = skill.DamageMult;
            template.KnockbackMult = skill.KnockbackMult;
            template.BlockDamageTypes = skill.BlockTypes;
            template.CounterDamageTypes = skill.CounterTypes;
            template.MaxRange = skill.MaxRange;
            template.TurnTowardsDealer = skill.TurnTowardsDealer;
        }
    }
}
