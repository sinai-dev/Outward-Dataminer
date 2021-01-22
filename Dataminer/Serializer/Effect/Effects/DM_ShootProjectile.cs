using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_ShootProjectile : DM_Shooter
    {
        public string BaseProjectile;
        //public List<DM_ProjectileShot> ProjectileShots = new List<DM_ProjectileShot>();
        public int ProjectileShots;

        public float Lifespan;
        public int InstantiatedAmount;
        public float LateShootTime;
        public bool Unblockable;
        public bool EffectsOnlyIfHitCharacter;
        public Projectile.EndLifeMode EndMode;
        public bool DisableOnHit;
        public bool IgnoreShooterCollision;

        public ShootProjectile.TargetMode TargetingMode;
        public int TargetCountPerProjectile;
        public float TargetRange;
        public bool AutoTarget;
        public float AutoTargetMaxAngle;
        public float AutoTargetRange;

        public List<DM_EffectTransform> ProjectileEffects = new List<DM_EffectTransform>();

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var template = holder as DM_ShootProjectile;
            var comp = effect as ShootProjectile;

            if (comp.BaseProjectile is Projectile projectile)
            {
                template.BaseProjectile = projectile.name;

                template.AutoTarget = comp.AutoTarget;
                template.AutoTargetMaxAngle = comp.AutoTargetMaxAngle;
                template.AutoTargetRange = comp.AutoTargetRange;
                template.IgnoreShooterCollision = comp.IgnoreShooterCollision;
                template.TargetCountPerProjectile = comp.TargetCountPerProjectile;
                template.TargetingMode = comp.TargetingMode;
                template.TargetRange = comp.TargetRange;
                template.InstantiatedAmount = comp.IntanstiatedAmount;

                template.DisableOnHit = projectile.DisableOnHit;
                template.EffectsOnlyIfHitCharacter = projectile.EffectsOnlyIfHitCharacter;
                template.EndMode = projectile.EndMode;
                template.LateShootTime = projectile.LateShootTime;
                template.Lifespan = projectile.Lifespan;
                template.Unblockable = projectile.Unblockable;

                if (comp.ProjectileShots != null)
                    template.ProjectileShots = comp.ProjectileShots.Length;

                foreach (Transform child in projectile.transform)
                {
                    var effectsChild = DM_EffectTransform.ParseTransform(child);

                    if (effectsChild.ChildEffects.Count > 0 || effectsChild.Effects.Count > 0 || effectsChild.EffectConditions.Count > 0)
                    {
                        template.ProjectileEffects.Add(effectsChild);
                    }
                }
            }
        }

        //public class DM_ProjectileShot
        //{
        //    public Vector3 LocalDirectionOffset;
        //    public Vector3 LockDirection;
        //    public bool MustShoot;
        //    public bool NoBaseDir;
        //    public Vector3 RandomLocalDirectionAdd;
        //}
    }
}
