using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ThrowItem : DM_ShootProjectile
    {
        public ProjectileItem.CollisionBehaviorTypes CollisionBehaviour;
        public ThrowItem.ProjectileBehaviorTypes ProjectileBehaviour;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var template = holder as DM_ThrowItem;
            var comp = effect as ThrowItem;

            template.CollisionBehaviour = comp.CollisionBehavior;
            template.ProjectileBehaviour = comp.ProjectileBehavior;
        }
    }
}
