using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_TrinketSkill : DM_Skill
    {
        public List<int> CompatibleItemIDs;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_TrinketSkill;
            var skill = item as TrinketSkill;

            template.CompatibleItemIDs = new List<int>();
            foreach (var compatItem in skill.CompatibleItems)
            {
                template.CompatibleItemIDs.Add(compatItem.ItemID);
            }
        }
    }
}
