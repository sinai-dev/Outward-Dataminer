using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_ImbueEffect
    {
        public int StatusID;

        public string Name;
        public string Description;

        public List<DM_EffectTransform> Effects;

        public static DM_ImbueEffect ParseImbueEffect(ImbueEffectPreset imbue)
        {
            var template = new DM_ImbueEffect
            {
                StatusID = imbue.PresetID,
                Name = imbue.Name,
                Description = imbue.Description
            };

            template.Effects = new List<DM_EffectTransform>();
            foreach (Transform child in imbue.transform)
            {
                var effectsChild = DM_EffectTransform.ParseTransform(child);

                if (effectsChild.ChildEffects.Count > 0 || effectsChild.Effects.Count > 0) // || effectsChild.EffectConditions.Count > 0)
                {
                    template.Effects.Add(effectsChild);
                }
            }

            return template;
        }
    }
}
