using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Dataminer
{

    public class DM_CounterSuccessCondition : DM_EffectCondition
    {

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            // Don't actually need to do anything for this effect. Simply adding the component is the entirety of the effect.
        }
    }
}
