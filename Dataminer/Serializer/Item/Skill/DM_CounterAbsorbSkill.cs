using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_CounterAbsorbSkill : DM_CounterSkill
    {
        public List<AbsorbType> Absorbs;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var skill = item as CounterAbsorbSkill;
            var template = holder as DM_CounterAbsorbSkill;

            template.Absorbs = new List<AbsorbType>();
            foreach (var absorb in skill.Absorbs)
            {
                template.Absorbs.Add(new AbsorbType()
                {
                    Condition = absorb.Condition != null ? (DM_BooleanCondition)DM_EffectCondition.ParseCondition(absorb.Condition) : null,
                    DamageTypes = absorb.Types
                });
            }
        }

        [DM_Serialized]
        public class AbsorbType
        {
            public DM_BooleanCondition Condition;
            public List<DamageType.Types> DamageTypes;
        }
    }
}
