using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_EffectTransform
    {
        public string TransformName = "";

        public List<DM_Effect> Effects = new List<DM_Effect>();
        public List<DM_EffectCondition> EffectConditions = new List<DM_EffectCondition>();
        public List<DM_EffectTransform> ChildEffects = new List<DM_EffectTransform>();

        public static DM_EffectTransform ParseTransform(Transform transform)
        {
            var effectTransformHolder = new DM_EffectTransform
            {
                TransformName = transform.name
            };

            foreach (Effect effect in transform.GetComponents<Effect>())
            {
                if (!effect.enabled)
                {
                    continue;
                }

                if (DM_Effect.ParseEffect(effect) is DM_Effect holder)
                {
                    effectTransformHolder.Effects.Add(holder);
                }
            }

            foreach (EffectCondition condition in transform.GetComponents<EffectCondition>())
            {
                var effectConditionHolder = DM_EffectCondition.ParseCondition(condition);
                effectTransformHolder.EffectConditions.Add(effectConditionHolder);
            }

            foreach (Transform child in transform)
            {
                if (child.name == "ExplosionFX" || child.name == "ProjectileFX")
                {
                    // visual effects, we dont care about these
                    continue;
                }

                var transformHolder = ParseTransform(child);
                if (transformHolder.ChildEffects.Count > 0 || transformHolder.Effects.Count > 0 || transformHolder.EffectConditions.Count > 0)
                {
                    effectTransformHolder.ChildEffects.Add(transformHolder);
                }
            }

            return effectTransformHolder;
        }
    }
}
