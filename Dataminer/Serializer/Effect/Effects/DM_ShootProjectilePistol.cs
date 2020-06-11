using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ShootProjectilePistol : DM_ShootProjectile
    {
        public bool UseShot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            (holder as DM_ShootProjectilePistol).UseShot = (effect as ShootProjectilePistol).UseShot;
        }
    }
}
