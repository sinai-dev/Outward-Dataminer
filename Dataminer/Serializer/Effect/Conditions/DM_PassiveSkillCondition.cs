using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_PassiveSkillCondition : DM_EffectCondition
    {
        public int ReqSkillID;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_PassiveSkillCondition).Invert = (component as PassiveSkillCondition).Inverse;
            (template as DM_PassiveSkillCondition).ReqSkillID = (component as PassiveSkillCondition).PassiveSkill.ItemID;
        }
    }
}
