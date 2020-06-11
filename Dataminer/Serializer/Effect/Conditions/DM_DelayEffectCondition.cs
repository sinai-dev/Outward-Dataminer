using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_DelayEffectCondition : DM_EffectCondition
    {
        public float Delay;
        public DelayEffectCondition.DelayTypes DelayType;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_DelayEffectCondition).Delay = (component as DelayEffectCondition).Delay;
            (template as DM_DelayEffectCondition).DelayType = (component as DelayEffectCondition).DelayType;
        }
    }
}
