using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_AttackSkill : DM_Skill
    {
        public List<Weapon.WeaponType> AmmunitionTypes = new List<Weapon.WeaponType>();
        public List<Weapon.WeaponType> RequiredOffHandTypes = new List<Weapon.WeaponType>();
        public List<Weapon.WeaponType> RequiredWeaponTypes = new List<Weapon.WeaponType>();
        public List<string> RequiredWeaponTags = new List<string>();
        public bool RequireImbue;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_AttackSkill;
            var attackSkill = item as AttackSkill;

            template.AmmunitionTypes = attackSkill.AmmunitionTypes;
            template.RequiredOffHandTypes = attackSkill.RequiredOffHandTypes;
            template.RequiredWeaponTypes = attackSkill.RequiredWeaponTypes;
            template.RequireImbue = attackSkill.RequireImbue;

            foreach (var tag in attackSkill.RequiredTags)
            {
                template.RequiredWeaponTags.Add(tag.Tag.TagName);
            }
        }
    }
}
