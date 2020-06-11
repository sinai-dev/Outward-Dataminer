using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [Obsolete("This class requires the use of ItemExtensions (WeaponLoadoutItem), so I haven't implemented it yet.")]
    public class DM_ShootItem : DM_ShootProjectile
    {
        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);
        }
    }
}
