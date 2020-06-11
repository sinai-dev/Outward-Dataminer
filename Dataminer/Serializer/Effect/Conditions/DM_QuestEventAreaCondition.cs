using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_QuestEventAreaCondition : DM_EffectCondition
    {
        public List<string> EventUIDs = new List<string>();

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var holder = template as DM_QuestEventAreaCondition;
            var comp = component as QuestEventAreaCondition;

            foreach (var _event in comp.EventToCheck)
            {
                holder.EventUIDs.Add(_event.EventUID);
            }
        }
    }
}
