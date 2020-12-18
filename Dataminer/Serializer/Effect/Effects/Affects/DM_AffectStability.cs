using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectStability : DM_Effect
    {
        public float AffectQuantity;
        public bool SetStability;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            AffectQuantity = (effect as AffectStability).AffectQuantity;
            SetStability = (effect as AffectStability).SetStability;
        }
    }
}
