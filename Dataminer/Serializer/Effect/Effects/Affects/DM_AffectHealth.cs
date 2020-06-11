using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectHealth : DM_Effect
    {
        public float AffectQuantity;
        public bool IsModifier;
        public float AffectQuantityAI;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectHealth).AffectQuantity = (effect as AffectHealth).AffectQuantity;
            (holder as DM_AffectHealth).AffectQuantityAI = (effect as AffectHealth).AffectQuantityOnAI;
            (holder as DM_AffectHealth).IsModifier = (effect as AffectHealth).IsModifier;
        }
    }
}
