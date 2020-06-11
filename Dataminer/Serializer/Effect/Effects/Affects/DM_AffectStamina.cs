using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectStamina : DM_Effect
    {
        public float AffectQuantity;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectStamina).AffectQuantity = (effect as AffectStamina).AffectQuantity;
        }
    }
}
