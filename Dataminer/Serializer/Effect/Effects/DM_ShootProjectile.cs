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
        public List<DM_ProjectileShot> ProjectileShots = new List<DM_ProjectileShot>();

        public float Lifespan;
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

        public float ProjectileForce;
        public Vector3 AddDirection;
        public Vector3 AddRotationForce;
        public float YMagnitudeAffect;
        public float YMagnitudeForce;
        public float DefenseLength;
        public float DefenseRange;

        //public int PhysicsLayerMask;
        //public bool OnlyExplodeOnLayerMask;

        public EquipmentSoundMaterials ImpactSoundMaterial;
        public Vector2 LightIntensityFade;
        public Vector3 PointOffset;
        public bool TrailEnabled;
        public float TrailTime;

        public List<DM_EffectTransform> ProjectileEffects = new List<DM_EffectTransform>();

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var template = holder as DM_ShootProjectile;
            var comp = effect as ShootProjectile;

            if (comp.BaseProjectile is Projectile projectile)
            {
                template.BaseProjectile = projectile.name;

                template.AddDirection = comp.AddDirection;
                template.AddRotationForce = comp.AddRotationForce;
                template.AutoTarget = comp.AutoTarget;
                template.AutoTargetMaxAngle = comp.AutoTargetMaxAngle;
                template.AutoTargetRange = comp.AutoTargetRange;
                template.IgnoreShooterCollision = comp.IgnoreShooterCollision;
                template.ProjectileForce = comp.ProjectileForce;
                template.TargetCountPerProjectile = comp.TargetCountPerProjectile;
                template.TargetingMode = comp.TargetingMode;
                template.TargetRange = comp.TargetRange;
                template.YMagnitudeAffect = comp.YMagnitudeAffect;
                template.YMagnitudeForce = comp.YMagnitudeForce;

                template.DefenseLength = projectile.DefenseLength;
                template.DefenseRange = projectile.DefenseRange;
                template.DisableOnHit = projectile.DisableOnHit;
                template.EffectsOnlyIfHitCharacter = projectile.EffectsOnlyIfHitCharacter;
                template.EndMode = projectile.EndMode;
                //template.OnlyExplodeOnLayerMask = projectile.OnlyExplodeOnLayers;
                //template.PhysicsLayerMask = projectile.ExplodeOnContactWithLayers.value;
                template.ImpactSoundMaterial = projectile.ImpactSoundMaterial;
                template.LateShootTime = projectile.LateShootTime;
                template.Lifespan = projectile.Lifespan;
                template.LightIntensityFade = projectile.LightIntensityFade;
                template.PointOffset = projectile.PointOffset;
                template.TrailEnabled = projectile.TrailEnabled;
                template.TrailTime = projectile.TrailTime;
                template.Unblockable = projectile.Unblockable;

                foreach (var shot in comp.ProjectileShots)
                {
                    template.ProjectileShots.Add(new DM_ProjectileShot()
                    {
                        RandomLocalDirectionAdd = shot.RandomLocalDirectionAdd,
                        LocalDirectionOffset = shot.LocalDirectionOffset,
                        LockDirection = shot.LockDirection,
                        MustShoot = shot.MustShoot,
                        NoBaseDir = shot.NoBaseDir
                    });
                }

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

        public class DM_ProjectileShot
        {
            public Vector3 LocalDirectionOffset;
            public Vector3 LockDirection;
            public bool MustShoot;
            public bool NoBaseDir;
            public Vector3 RandomLocalDirectionAdd;
        }
    }
}
