using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_AffectDrink : DM_Effect
    {
        public float AffectQuantity;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectDrink).AffectQuantity = (float)At.GetValue(typeof(AffectNeed), effect, "m_affectQuantity");
        }
    }
}
