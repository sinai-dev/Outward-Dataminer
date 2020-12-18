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

        public int AmmunitionAmount;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var attackSkill = item as AttackSkill;

            AmmunitionTypes = attackSkill.AmmunitionTypes;
            RequiredOffHandTypes = attackSkill.RequiredOffHandTypes;
            RequiredWeaponTypes = attackSkill.RequiredWeaponTypes;
            RequireImbue = attackSkill.RequireImbue;

            AmmunitionAmount = attackSkill.AmmunitionAmount;

            foreach (var tag in attackSkill.RequiredTags)
            {
                RequiredWeaponTags.Add(tag.Tag.TagName);
            }
        }
    }
}
