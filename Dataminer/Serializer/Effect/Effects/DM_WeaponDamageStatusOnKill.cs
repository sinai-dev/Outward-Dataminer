using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_WeaponDamageStatusOnKill : DM_WeaponDamage
    {
        public string StatusIdentifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            StatusIdentifier = (effect as WeaponDamageStatusOnKill).Status?.IdentifierName;
        }
    }
}
