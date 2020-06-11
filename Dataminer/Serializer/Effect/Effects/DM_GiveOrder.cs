using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_GiveOrder : DM_Effect
    {
        public int OrderID;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_GiveOrder).OrderID = (effect as GiveOrder).OrderID;
        }
    }
}
