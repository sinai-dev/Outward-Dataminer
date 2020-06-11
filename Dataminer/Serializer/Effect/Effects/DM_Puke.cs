using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_Puke : DM_Effect
    {
        public int ChanceToTrigger;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_Puke).ChanceToTrigger = (effect as Puke).ChancesToTrigger;
        }
    }
}
