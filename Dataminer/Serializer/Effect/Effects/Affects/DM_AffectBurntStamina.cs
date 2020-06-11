using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectBurntStamina : DM_Effect
    {
        public float AffectQuantity;
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectBurntStamina).AffectQuantity = (effect as AffectBurntStamina).AffectQuantity;
            (holder as DM_AffectBurntStamina).IsModifier = (effect as AffectBurntStamina).IsModifier;
        }
    }
}