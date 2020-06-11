using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_DealtDamageCondition : DM_EffectCondition
    {
        public List<DM_Damage> RequiredDamages = new List<DM_Damage>();

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            this.RequiredDamages = DM_Damage.ParseDamageArray((component as DealtDamageCondition).DealtDamages);
        }
    }
}
