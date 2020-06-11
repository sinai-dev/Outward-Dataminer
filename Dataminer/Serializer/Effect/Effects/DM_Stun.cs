using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_Stun : DM_Effect
    {
        public float Duration;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_Stun).Duration = (effect as Stun).Duration;
        }
    }
}
