using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_CorruptionLevelCondition : DM_EffectCondition
    {
        public float Value;
        public AICondition.NumericCompare CompareType = AICondition.NumericCompare.Equal;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as CorruptionLevelCondition;

            Value = comp.Value;
            CompareType = comp.CompareType;
        }
    }
}
