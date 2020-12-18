using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_PackedInstrumentCharges : DM_EffectCondition
    {
        public int Min;
        public int Max;
        public int PrePackDeployableID;
        public int InstrumentID;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as PackedInstrumentCharges;

            Min = comp.Min;
            Max = comp.Max;
            PrePackDeployableID = comp.PackDeployable?.ID ?? -1;
            InstrumentID = comp.Instrument?.ItemID ?? -1;
        }
    }
}
