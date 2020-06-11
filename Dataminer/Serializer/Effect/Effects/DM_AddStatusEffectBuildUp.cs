using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AddStatusEffectBuildUp : DM_Effect
    {
        public string StatusEffect = "";
        public float Buildup;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var addStatusEffectBuildUp = effect as AddStatusEffectBuildUp;
            var addStatusEffectBuildupHolder = holder as DM_AddStatusEffectBuildUp;

            if (addStatusEffectBuildUp.Status)
            {
                addStatusEffectBuildupHolder.StatusEffect = addStatusEffectBuildUp.Status.IdentifierName;
                addStatusEffectBuildupHolder.Buildup = addStatusEffectBuildUp.BuildUpValue;
            }
        }
    }
}
