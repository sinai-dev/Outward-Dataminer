using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_VitalityDamage : DM_Effect
    {
        public float PercentOfMax;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_VitalityDamage).PercentOfMax = (effect as VitalityDamage).PercentOfMax;
        }
    }
}
