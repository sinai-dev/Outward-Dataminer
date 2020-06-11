using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectBurntHealth : DM_Effect
    {
        public float AffectQuantity;
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectBurntHealth).AffectQuantity = (effect as AffectBurntHealth).AffectQuantity;
            (holder as DM_AffectBurntHealth).IsModifier = (effect as AffectBurntHealth).IsModifier;
        }
    }
}
