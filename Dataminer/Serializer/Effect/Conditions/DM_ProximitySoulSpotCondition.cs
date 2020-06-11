using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ProximitySoulSpotCondition : DM_EffectCondition
    {
        public float Distance;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_ProximitySoulSpotCondition).Distance = (component as ProximitySoulSpotCondition).ProximityDist;
        }
    }
}
