using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ShootEnchantmentBlast : DM_ShootBlast
    {
        public float Multiplier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            Multiplier = (effect as ShootEnchantmentBlast).DamageMultiplier;
        }
    }
}
