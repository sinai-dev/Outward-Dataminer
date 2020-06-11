using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_HasQuantityItemsCondition : DM_EffectCondition
    {
        public int TotalItemsRequired;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_HasQuantityItemsCondition).TotalItemsRequired = (component as HasQuantityItemsCondition).ItemQuantityRequired;
        }
    }
}
