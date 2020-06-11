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
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectStability).AffectQuantity = (effect as AffectStability).AffectQuantity;
            (holder as DM_AffectStability).IsModifier = (effect as AffectStability).IsModifier;
        }
    }
}
