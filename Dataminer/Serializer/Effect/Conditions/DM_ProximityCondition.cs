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
        public float ProximityAngle;
        public bool OrMode;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as ProximityCondition;

            MaxDistance = comp.ProximityDist;
            Offset = comp.Offset;
            ProximityAngle = comp.ProximityAngle;
            OrMode = comp.OrMode;

            foreach (var req in comp.ProximityItemReq)
            {
                RequiredItems.Add(new DM_Skill.SkillItemReq()
                {
                    Consume = req.Consume,
                    Quantity = req.Quantity,
                    ItemID = req.Item.ItemID
                });
            }
        }
    }
}
