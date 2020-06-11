using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AddStatusEffect : DM_Effect
    {
        public string StatusEffect = "";
        public int ChanceToContract;    

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            if ((effect as AddStatusEffect).Status)
            {
                (holder as DM_AddStatusEffect).StatusEffect = (effect as AddStatusEffect).Status.IdentifierName;
                (holder as DM_AddStatusEffect).ChanceToContract = (effect as AddStatusEffect).BaseChancesToContract;
            }
        }
    }
}
