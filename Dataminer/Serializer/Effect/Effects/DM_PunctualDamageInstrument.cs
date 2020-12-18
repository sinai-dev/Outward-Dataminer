using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_PunctualDamageInstrument : DM_PunctualDamage
    {
        public float DamageCap;
        public float DamagePerCharge;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var comp = effect as PunctualDamageInstrument;

            DamageCap = comp.DamageCap;
            DamagePerCharge = comp.DamagePerCharge;
        }
    }
}
