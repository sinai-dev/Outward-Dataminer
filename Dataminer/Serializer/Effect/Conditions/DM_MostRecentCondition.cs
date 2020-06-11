using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_MostRecentCondition : DM_EffectCondition
    {
        public string StatusIdentifierToCheck;
        public string StatusIdentifierToCompareTo;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var holder = template as DM_MostRecentCondition;
            var comp = component as MostRecentCondition;

            holder.StatusIdentifierToCheck = comp.StatusEffectPrefab?.IdentifierName;
            holder.StatusIdentifierToCompareTo = comp.StatusEffectToCompare?.IdentifierName;
        }
    }
}
