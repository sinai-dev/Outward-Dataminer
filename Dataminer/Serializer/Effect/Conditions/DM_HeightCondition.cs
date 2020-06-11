using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_HeightCondition : DM_EffectCondition
    {
        public bool AllowEqual;
        public HeightCondition.CompareTypes CompareType;
        public float HeightThreshold;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as HeightCondition;
            var holder = template as DM_HeightCondition;

            holder.CompareType = comp.CompareType;
            holder.AllowEqual = comp.AllowEqual;
            holder.HeightThreshold = comp.HeightThreshold;
        }
    }
}
