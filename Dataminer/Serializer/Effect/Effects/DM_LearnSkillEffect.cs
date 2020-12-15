using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_LearnSkillEffect : DM_Effect
    {
        public int SkillID;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            SkillID = (effect as LearnSkillEffect).LearntSkill?.ItemID ?? -1;   
        }
    }
}
