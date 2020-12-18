using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_NoSameShooterBlastInProximity : DM_EffectCondition
    {
        public float Proximity;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            Proximity = (component as NoSameShooterBlastInProximity).Proximity;
        }
    }
}
