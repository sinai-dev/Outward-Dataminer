using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ContainedWaterCondition : DM_EffectCondition
    {
        public WaterType ValidWaterType;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_ContainedWaterCondition).ValidWaterType = (component as ContainedWaterCondition).ValidWaterType;
        }
    }
}
