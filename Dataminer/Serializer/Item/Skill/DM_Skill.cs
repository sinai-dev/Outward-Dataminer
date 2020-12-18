using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_Skill : DM_Item
    {
        public float Cooldown;
        public float StaminaCost;
        public float ManaCost;
        public float DurabilityCost;
        public float DurabilityCostPercent;

        public float HealthCost;
        public PlayerSystem.PlayerTypes RequiredPType;

        public List<SkillItemReq> RequiredItems = new List<SkillItemReq>();

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var skill = item as Skill;

            Cooldown = skill.Cooldown;
            StaminaCost = skill.StaminaCost;
            ManaCost = skill.ManaCost;
            DurabilityCost = skill.DurabilityCost;
            DurabilityCostPercent = skill.DurabilityCostPercent;

            HealthCost = skill.HealthCost;
            RequiredPType = skill.RequiredPType;

            if (skill.RequiredItems != null)
            {
                foreach (Skill.ItemRequired itemReq in skill.RequiredItems)
                {
                    if (itemReq.Item != null)
                    {
                        RequiredItems.Add(new SkillItemReq
                        {
                            ItemID = itemReq.Item.ItemID,
                            Consume = itemReq.Consume,
                            Quantity = itemReq.Quantity
                        });
                    }
                }
            }
        }

        [DM_Serialized]
        public class SkillItemReq
        {
            public int ItemID;
            public int Quantity;
            public bool Consume;
        }
    }
}
