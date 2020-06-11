using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_Disease : DM_StatusEffect
    {
        public Diseases DiseaseType;
        public int AutoHealTime;
        public bool CanDegenerate;
        public float DegenerateTime;
        public int StraightSleepHealTime;
        public bool CanBeHealedBySleeping;

        public override void SerializeStatusEffect(StatusEffect status, EffectPreset preset)
        {
            base.SerializeStatusEffect(status, preset);

            var disease = status as Disease;

            CanDegenerate = disease.CanHealByItself;
            StraightSleepHealTime = disease.StraightSleepHealTime;
            CanBeHealedBySleeping = disease.CanBeHealedBySleeping;
            AutoHealTime = (int)At.GetValue(typeof(Disease), disease, "m_autoHealTime");
            DegenerateTime = (float)At.GetValue(typeof(Disease), disease, "m_degenerateTime");
            DiseaseType = (Diseases)At.GetValue(typeof(Disease), disease, "m_diseasesType");
        }
    }
}
