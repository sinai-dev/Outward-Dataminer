using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_BooleanCondition : DM_EffectCondition
    {
        public bool Valid;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_BooleanCondition).Valid = (component as BooleanCondition).Valid;   
        }
    }
}
