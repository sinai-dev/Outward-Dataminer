using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_PunctualDamageAoE : DM_PunctualDamage
    {
        public float Radius;
        public Shooter.TargetTypes TargetType;
        public bool IgnoreShooter;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var template = holder as DM_PunctualDamageAoE;
            var comp = effect as PunctualDamageAoE;

            template.Radius = comp.Radius;
            template.TargetType = comp.TargetType;
            template.IgnoreShooter = comp.IgnoreShooter;
        }
    }
}
