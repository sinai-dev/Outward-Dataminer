using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_InstrumentTriggerBubble : DM_Effect
    {
        public float Range;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            Range = (effect as InstrumentTriggerBubble).Range;
        }
    }
}
