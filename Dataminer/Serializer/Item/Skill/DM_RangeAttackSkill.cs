using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_RangeAttackSkill : DM_AttackSkill
    {
        public bool AutoLoad;
        public bool FakeShoot;
        public bool OverrideAimOffset;
        public Vector2 AimOffset;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_RangeAttackSkill;
            var skill = item as RangeAttackSkill;

            template.AutoLoad = skill.AutoLoad;
            template.FakeShoot = skill.FakeShoot;
            template.OverrideAimOffset = skill.OverrideAimOffset;
            template.AimOffset = skill.AimOffset;
        }
    }
}
