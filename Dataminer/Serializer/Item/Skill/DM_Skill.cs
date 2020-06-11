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

        public List<SkillItemReq> RequiredItems = new List<SkillItemReq>();

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var skillHolder = holder as DM_Skill;
            var skill = item as Skill;

            skillHolder.Cooldown = skill.Cooldown;
            skillHolder.StaminaCost = skill.StaminaCost;
            skillHolder.ManaCost = skill.ManaCost;
            skillHolder.DurabilityCost = skill.DurabilityCost;
            skillHolder.DurabilityCostPercent = skill.DurabilityCostPercent;

            if (skill.RequiredItems != null)
            {
                foreach (Skill.ItemRequired itemReq in skill.RequiredItems)
                {
                    if (itemReq.Item != null)
                    {
                        skillHolder.RequiredItems.Add(new SkillItemReq
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
