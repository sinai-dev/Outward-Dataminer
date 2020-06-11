using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ProbabilityCondition : DM_EffectCondition
    {
        public int ChancePercent;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_ProbabilityCondition).ChancePercent = (component as ProbabilityCondition).ProbabilityChances;
        }
    }
}
