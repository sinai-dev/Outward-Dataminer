using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AddBoonEffect : DM_AddStatusEffect
    {
        public string AmplifiedEffect = "";

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            (holder as DM_AddBoonEffect).AmplifiedEffect = (effect as AddBoonEffect).BoonAmplification.IdentifierName;
        }
    }
}
