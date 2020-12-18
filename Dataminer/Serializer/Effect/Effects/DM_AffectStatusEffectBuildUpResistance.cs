using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_AffectStatusEffectBuildUpResistance : DM_Effect
    {
        public string StatusEffectIdentifier;
        public float Value;
        public float Duration;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as AffectStatusEffectBuildUpResistance;

            StatusEffectIdentifier = comp.StatusEffect?.IdentifierName;
            Value = comp.Value;
            Duration = comp.Duration;
        }
    }
}
