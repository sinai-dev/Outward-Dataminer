using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_ImbueProjectile : DM_ImbueObject
    {
        public bool UnloadProjectile;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            UnloadProjectile = (effect as ImbueProjectile).UnloadProjectile;
        }
    }
}
