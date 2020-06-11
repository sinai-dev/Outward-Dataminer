using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ShooterPosStatusEffect : DM_EffectCondition
    {
        public string StatusIdentifier;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_ShooterPosStatusEffect).StatusIdentifier = (component as ShooterPosStatusEffect).StatusEffect.IdentifierName;
        }
    }
}
