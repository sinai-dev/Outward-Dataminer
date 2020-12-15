using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_AchievementOnEffect : DM_Effect
    {
        public AchievementManager.Achievement UnlockedAchievement;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            UnlockedAchievement = (effect as AchievementOnEffect).UnlockedAchievement;
        }
    }
}
