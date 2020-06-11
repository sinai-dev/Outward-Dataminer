using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_RemoveStatusEffect : DM_Effect
    {
        public string Status_Name = "";
        public string Status_Tag = "";
        public RemoveStatusEffect.RemoveTypes CleanseType;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var template = holder as DM_RemoveStatusEffect;
            var comp = effect as RemoveStatusEffect;

            template.CleanseType = comp.CleanseType;

            if (template.CleanseType == RemoveStatusEffect.RemoveTypes.StatusSpecific && comp.StatusEffect)
            {
                template.Status_Name = comp.StatusEffect.IdentifierName;
            }
            else if (template.CleanseType == RemoveStatusEffect.RemoveTypes.StatusFamily && comp.StatusFamily != null)
            {
                template.Status_Name = comp.StatusFamily.SelectorValue;
            }
            else if (template.CleanseType == RemoveStatusEffect.RemoveTypes.StatusType)
            {
                template.Status_Tag = comp.StatusType?.Tag.TagName;
            }
            else if (template.CleanseType == RemoveStatusEffect.RemoveTypes.StatusNameContains)
            {
                template.Status_Name = comp.StatusName;
            }
        }
    }
}
