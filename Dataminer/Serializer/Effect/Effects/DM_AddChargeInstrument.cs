using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_AddChargeInstrument : DM_Effect
    {
        public int Charges;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as AddChargeInstrument;

            Charges = comp.Charges;
        }
    }
}
