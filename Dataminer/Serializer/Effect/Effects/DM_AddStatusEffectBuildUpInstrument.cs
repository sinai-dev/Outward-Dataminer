using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_AddStatusEffectBuildUpInstrument : DM_AddStatusEffectBuildUp
    {
        public float ChancesPerCharge;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var comp = effect as AddStatusEffectBuildUpInstrument;

            ChancesPerCharge = comp.ChancesPerCharge;
        }
    }
}
