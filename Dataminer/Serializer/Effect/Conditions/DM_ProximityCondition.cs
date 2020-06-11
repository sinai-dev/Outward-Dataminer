using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_ProximityCondition : DM_EffectCondition
    {
        public List<DM_Skill.SkillItemReq> RequiredItems = new List<DM_Skill.SkillItemReq>();
        public float MaxDistance;
        public Vector3 Offset = Vector3.zero;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var holder = template as DM_ProximityCondition;
            var comp = component as ProximityCondition;

            holder.MaxDistance = comp.ProximityDist;
            holder.Offset = comp.Offset;

            foreach (var req in comp.ProximityItemReq)
            {
                holder.RequiredItems.Add(new DM_Skill.SkillItemReq()
                {
                    Consume = req.Consume,
                    Quantity = req.Quantity,
                    ItemID = req.Item.ItemID
                });
            }
        }
    }
}
