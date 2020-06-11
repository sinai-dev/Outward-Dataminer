using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ShootBlastHornetControl : DM_ShootBlast
    {
        public int BurstSkillID;
        public int HealSkillID;

        public float Acceleration;
        public float Speed;
        public float SpeedDistLerpWhenCloseMult;

        public float DistStayOnTarget;
        public float EndEffectTriggerDist;
        public float EnvironmentCheckRadius;

        public float TimeFlight;
        public float TimeStayOnTarget;

        public float PassiveTimeFlight;
        public float PassiveTimeStayOnTarget;

        public float HornetLookForTargetRange;
        public float HornetPassiveAttackTimer;
        public float HornetPassiveTargetRange;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            var template = holder as DM_ShootBlastHornetControl;
            var comp = effect as ShootBlastHornetControl;

            template.BurstSkillID = comp.BurstSkill?.ItemID ?? -1;
            template.HealSkillID = comp.HealSkill?.ItemID ?? -1;

            template.Acceleration = comp.Acceleration;
            template.DistStayOnTarget = comp.DistStayOnTarget;
            template.EndEffectTriggerDist = comp.EndEffectTriggerDist;
            template.EnvironmentCheckRadius = comp.EnvironmentCheckRadius;
            template.HornetLookForTargetRange = comp.HornetLookForTargetRange;
            template.HornetPassiveAttackTimer = comp.HornetPassiveAttackTimer;
            template.HornetPassiveTargetRange = comp.HornetPassiveTargetRange;
            template.PassiveTimeFlight = comp.PassiveTimeFlight;
            template.PassiveTimeStayOnTarget = comp.PassiveTimeStayOnTarget;
            template.Speed = comp.Speed;
            template.SpeedDistLerpWhenCloseMult = comp.SpeedDistLerpWhenCloseMult;
            template.TimeFlight = comp.TimeFlight;
            template.TimeStayOnTarget = comp.TimeStayOnTarget;
        }
    }
}
