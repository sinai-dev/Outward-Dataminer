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
        public float BuildUpMultiplier;
        public bool BypassCounter;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as AddStatusEffectBuildUp;

            if (comp.Status)
                StatusEffect = comp.Status.IdentifierName;

            Buildup = comp.BuildUpValue;
            BuildUpMultiplier = comp.BuildUpMultiplier;
            BypassCounter = comp.BypassCounter;
        }
    }
}
