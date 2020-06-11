using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectBurntMana : DM_Effect
    {
        public float AffectQuantity;
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectBurntMana).AffectQuantity = (effect as AffectBurntMana).AffectQuantity;
            (holder as DM_AffectBurntMana).IsModifier = (effect as AffectBurntMana).IsModifier;
        }
    }
}
