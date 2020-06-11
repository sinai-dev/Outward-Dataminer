using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_AffectCorruption : DM_Effect
    {
        public float AffectQuantity;
        public bool IsRaw;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectCorruption).AffectQuantity = (effect as AffectCorruption).AffectQuantity;
            (holder as DM_AffectCorruption).IsRaw = (effect as AffectCorruption).IsRaw;
        }
    }
}
