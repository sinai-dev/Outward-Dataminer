using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ThrowSkill : DM_AttackSkill
    {
        public List<int> ThrowableItemIDs;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_ThrowSkill;
            var skill = item as ThrowSkill;

            template.ThrowableItemIDs = new List<int>();
            foreach (var throwable in skill.ThrowableItems)
            {
                template.ThrowableItemIDs.Add(throwable.ItemID);
            }
        }
    }
}
