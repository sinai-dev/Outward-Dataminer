using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_HasStatusLevelCondition : DM_EffectCondition
    {
        public string StatusIdentifier;
        public int CompareLevel;
        public bool CheckOwner;
        public AICondition.NumericCompare ComparisonType;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as HasStatusLevelCondition;
            var holder = template as DM_HasStatusLevelCondition;

            holder.StatusIdentifier = comp.StatusEffect?.IdentifierName;
            holder.CheckOwner = comp.CheckOwner;
            holder.CompareLevel = comp.CompareLevel;
            holder.ComparisonType = comp.ComparaisonType;
        }
    }
}
