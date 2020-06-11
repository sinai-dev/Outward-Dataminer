using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ReduceStatusLevel : DM_Effect
    {
        public string StatusIdentifierToReduce;
        public int ReduceAmount;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_ReduceStatusLevel).ReduceAmount = (effect as ReduceStatusLevel).LevelAmount;
            (holder as DM_ReduceStatusLevel).StatusIdentifierToReduce = (effect as ReduceStatusLevel).StatusEffectToReduce.IdentifierName;
        }
    }
}
