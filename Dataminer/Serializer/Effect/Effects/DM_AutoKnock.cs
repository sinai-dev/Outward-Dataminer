using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_AutoKnock : DM_Effect
    {
        public bool KnockDown;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AutoKnock).KnockDown = (effect as AutoKnock).Down;
        }
    }
}
