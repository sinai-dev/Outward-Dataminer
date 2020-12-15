using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer
{
    public class DM_AchievementSetStatOnEffect : DM_Effect
    {
        public AchievementManager.AchievementStat StatToChange;
        public int IncreaseAmount;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as AchievementSetStatOnEffect;

            StatToChange = comp.StatToChange;
            IncreaseAmount = comp.IncreaseAmount;
        }
    }
}
