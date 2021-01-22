using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_ShootBlast : DM_Shooter
    {
        public string BaseBlast;
        public float Radius;
        public float RefreshTime;
        public float BlastLifespan;
        public int InstantiatedAmount;

        public bool Interruptible;
        public int MaxHitTargetCount;
        public bool AffectHitTargetCenter;
        public bool HitOnShoot;
        public bool IgnoreShooter;

        public List<DM_EffectTransform> BlastEffects = new List<DM_EffectTransform>();

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var template = holder as DM_ShootBlast;
            var comp = effect as ShootBlast;

            if (comp.BaseBlast is Blast blast)
            {
                template.BaseBlast = blast.name;
                template.AffectHitTargetCenter = blast.AffectHitTargetCenter;
                template.HitOnShoot = blast.HitOnShoot;
                template.IgnoreShooter = blast.IgnoreShooter;
                template.Interruptible = blast.Interruptible;
                template.MaxHitTargetCount = blast.MaxHitTargetCount;
                template.Radius = blast.Radius;
                template.RefreshTime = blast.RefreshTime;

                template.BlastLifespan = comp.BlastLifespan;
                template.InstantiatedAmount = comp.InstanstiatedAmount;

                foreach (Transform child in blast.transform)
                {
                    var effectsChild = DM_EffectTransform.ParseTransform(child);

                    if (effectsChild.ChildEffects.Count > 0 || effectsChild.Effects.Count > 0 || effectsChild.EffectConditions.Count > 0)
                    {
                        template.BlastEffects.Add(effectsChild);
                    }
                }
            }
        }
    }
}
