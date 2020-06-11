using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectHealthParentOwner : DM_Effect
    {
        public float AffectQuantity;
        public bool Requires_AffectedChar;
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectHealthParentOwner).AffectQuantity = (effect as AffectHealthParentOwner).AffectQuantity;
            (holder as DM_AffectHealthParentOwner).Requires_AffectedChar = (effect as AffectHealthParentOwner).OnlyIfHasAffectedChar;
            (holder as DM_AffectHealthParentOwner).IsModifier = (effect as AffectHealthParentOwner).IsModifier;
        }
    }
}
