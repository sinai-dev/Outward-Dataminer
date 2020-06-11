using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    // This EffectCondition has no fields, the name itself implies everything you need to know.

    public class DM_IsWorldHostCondition : DM_EffectCondition
    {
        public override void SerializeEffect<T>(EffectCondition component, T template) { }
    }
}
