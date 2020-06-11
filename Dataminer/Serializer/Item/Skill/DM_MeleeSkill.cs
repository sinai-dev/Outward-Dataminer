using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_MeleeSkill : DM_AttackSkill
    {
        public bool Blockable;
        public float Damage;
        public float Impact;
        public int LinecastCount;
        public float Radius;
        public bool Unblockable;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var skill = item as MeleeSkill;
            var template = holder as DM_MeleeSkill;

            template.Blockable = skill.Blockable;

            if (skill.MeleeHitDetector is MeleeHitDetector detector)
            {
                template.Damage = detector.Damage;
                template.Impact = detector.Impact;
                template.LinecastCount = detector.LinecastCount;
                template.Radius = detector.Radius;
                template.Unblockable = detector.Unblockable;
            }
        }
    }
}
