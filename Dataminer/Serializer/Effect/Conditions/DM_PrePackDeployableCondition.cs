using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_PrePackDeployableCondition : DM_EffectCondition
    {
        public float ProximityDist;
        public float ProximityAngle;
        public int[] PackableItemIDs;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as PrePackDeployableCondition;

            ProximityAngle = comp.ProximityAngle;
            ProximityDist = comp.ProximityDist;
            PackableItemIDs = comp.PackableItems?.Select(it => it?.ItemID ?? -1).ToArray();
        }
    }
}
