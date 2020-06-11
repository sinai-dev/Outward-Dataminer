using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_HasStatusEffectEffectCondition : DM_EffectCondition
    {
        public float DiseaseAge;
        public bool CheckOwner;

        // StatusSpecific, StatusFamily, StatusType
        public StatusEffectSelector.Types StatusSelectorType;

        public string SelectorValue;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as HasStatusEffectEffectCondition;
            var holder = template as DM_HasStatusEffectEffectCondition;

            holder.Invert = comp.Inverse;
            holder.DiseaseAge = comp.DiseaseAge;
            holder.CheckOwner = comp.CheckOwner;

            var selector = comp.StatusEffect;

            holder.StatusSelectorType = selector.Type;

            switch (selector.Type)
            {
                case StatusEffectSelector.Types.StatusFamily:
                    holder.SelectorValue = StatusEffectFamilyLibrary.Instance.GetStatusEffect(selector.StatusFamily.SelectorValue).UID;
                    break;

                case StatusEffectSelector.Types.StatusSpecific:
                    holder.SelectorValue = selector.StatusEffect?.IdentifierName;
                    break;

                case StatusEffectSelector.Types.StatusType:
                    holder.SelectorValue = selector.StatusType.Tag.TagName;
                    break;
            }
        }
    }
}
