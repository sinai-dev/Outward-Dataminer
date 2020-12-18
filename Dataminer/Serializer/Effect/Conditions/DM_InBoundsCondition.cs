using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dataminer
{
    public class DM_InBoundsCondition : DM_EffectCondition
    {
        public Bounds Bounds;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            Bounds = (component as InBoundsCondition).Bounds;
        }
    }
}
