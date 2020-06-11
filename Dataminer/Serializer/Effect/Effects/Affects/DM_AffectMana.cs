using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectMana : DM_Effect
    {
        public float AffectQuantity;
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectMana).AffectQuantity = (effect as AffectMana).Value;
            (holder as DM_AffectMana).IsModifier = (effect as AffectMana).IsModifier;
        }
    }
}
