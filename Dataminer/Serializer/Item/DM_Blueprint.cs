using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideLoader;

namespace Dataminer
{
    public class DM_Blueprint : DM_Item
    {
        public int DeployableItemID;
        public bool IsUpgrade;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var comp = item as Blueprint;

            DeployableItemID = comp.GetComponent<Deployable>()?.Item?.ItemID ?? -1;
            IsUpgrade = comp.IsUpgrade;
        }
    }
}
