using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideLoader;

namespace Dataminer
{
    public class DM_AddAllStatusEffectBuildUp : DM_Effect
    {
        public float BuildUpValue;
        public bool NoDealer;
        public bool AffectController;
        public float BuildUpBonus;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as AddAllStatusEffectBuildUp;

            BuildUpValue = comp.BuildUpValue;
            NoDealer = comp.NoDealer;
            AffectController = comp.AffectController;
            BuildUpBonus = (float)At.GetField(comp, "m_buildUpBonus");
        }
    }
}
