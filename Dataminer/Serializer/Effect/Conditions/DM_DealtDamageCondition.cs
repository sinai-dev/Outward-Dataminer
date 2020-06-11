using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_DealtDamageCondition : DM_EffectCondition
    {
        public List<Damages> RequiredDamages = new List<Damages>();

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            this.RequiredDamages = Damages.ParseDamageArray((component as DealtDamageCondition).DealtDamages);
        }
    }
}
