using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_AddAbsorbHealth : DM_Effect
    {
        public float Ratio;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            Ratio = (effect as AddAbsorbHealth).Ratio;
        }
    }
}
