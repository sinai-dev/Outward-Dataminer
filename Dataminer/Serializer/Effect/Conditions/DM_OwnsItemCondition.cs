using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_OwnsItemCondition : DM_EffectCondition
    {
        public int ReqItemID;
        public int ReqAmount;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_OwnsItemCondition).ReqItemID = (component as OwnsItemCondition).ReqItem.ItemID;
            (template as DM_OwnsItemCondition).ReqAmount = (component as OwnsItemCondition).MinAmount;
        }
    }
}
