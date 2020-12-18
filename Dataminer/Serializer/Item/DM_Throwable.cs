using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_Throwable : DM_Item
    {
        public float DestroyDelay;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var comp = item as Throwable;

            DestroyDelay = comp.DestroyDelay;
        }
    }
}
