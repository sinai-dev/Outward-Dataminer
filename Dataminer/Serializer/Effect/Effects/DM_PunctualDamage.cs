using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_PunctualDamage : DM_Effect
    {
        public List<Damages> Damage = new List<Damages>();
        public List<Damages> Damages_AI = new List<Damages>();
        public float Knockback;
        public bool HitInventory;
        public bool IgnoreHalfResistances;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var puncDamage = effect as PunctualDamage;

            Knockback = puncDamage.Knockback;
            HitInventory = puncDamage.HitInventory;
            Damage = Damages.ParseDamageArray(puncDamage.Damages);
            Damages_AI = Damages.ParseDamageArray(puncDamage.DamagesAI);
            IgnoreHalfResistances = puncDamage.IgnoreHalfResistances;
        }
    }
}
