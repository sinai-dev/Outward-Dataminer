using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_InstrumentClose : DM_EffectCondition
    {
        public int MainInstrumentID;
        public int[] OtherInstrumentIDs;
        public float Range;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as InstrumentClose;

            MainInstrumentID = comp.Instrument?.ItemID ?? -1;
            OtherInstrumentIDs = comp.OtherInstruments?.Select(it => it?.ItemID ?? -1).ToArray();
            Range = comp.Range;
        }
    }
}
