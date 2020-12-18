using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_MeleeSkill : DM_AttackSkill
    {
        public bool Blockable;
        public bool NoWeaponAtkTag;

        public float Damage;
        public float Impact;
        public int LinecastCount;
        public float Radius;
        public bool Unblockable;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var skill = item as MeleeSkill;

            Blockable = skill.Blockable;
            NoWeaponAtkTag = skill.NoWeaponAtkTag;

            if (skill.MeleeHitDetector is MeleeHitDetector detector)
            {
                Damage = detector.Damage;
                Impact = detector.Impact;
                LinecastCount = detector.LinecastCount;
                Radius = detector.Radius;
                Unblockable = detector.Unblockable;
            }
        }
    }
}
