using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_Cough : DM_Effect
    {
        public int ChanceToTrigger;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_Cough).ChanceToTrigger = (effect as Cough).ChancesToTrigger;
        }
    }
}
