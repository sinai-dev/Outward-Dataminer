using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_CreateItemEffect : DM_Effect
    {
        public int ItemToCreate;
        public int Quantity = 1;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as CreateItemEffect;

            ItemToCreate = comp.ItemToCreate?.ItemID ?? -1;
            Quantity = comp.Quantity;
        }
    }
}
