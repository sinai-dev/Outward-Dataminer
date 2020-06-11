using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_StatusEffectCondition : DM_EffectCondition
    {
        public string StatusIdentifier;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var holder = template as DM_StatusEffectCondition;
            var comp = component as StatusEffectCondition;

            holder.StatusIdentifier = comp.StatusEffectPrefab.IdentifierName;
            holder.Invert = comp.Inverse;
        }
    }
}
