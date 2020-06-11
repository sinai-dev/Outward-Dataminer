using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_AttackTypeCondition : DM_EffectCondition
    {
        List<int> AffectOnAttackIDs = new List<int>();

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_AttackTypeCondition).AffectOnAttackIDs = (component as AttackTypeCondition).AffectOnAttacks.ToList();
        }
    }
}
