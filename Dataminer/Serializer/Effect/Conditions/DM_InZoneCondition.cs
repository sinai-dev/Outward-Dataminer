using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_InZoneCondition : DM_EffectCondition
    {
        public float Radius;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_InZoneCondition).Radius = (component as InZoneCondition).Radius;
        }
    }
}
