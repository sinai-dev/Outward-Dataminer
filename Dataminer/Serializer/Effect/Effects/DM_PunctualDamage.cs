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

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var puncDamage = effect as PunctualDamage;
            var puncHolder = holder as DM_PunctualDamage;

            puncHolder.Knockback = puncDamage.Knockback;
            puncHolder.HitInventory = puncDamage.HitInventory;
            puncHolder.Damage = Damages.ParseDamageArray(puncDamage.Damages);
            puncHolder.Damages_AI = Damages.ParseDamageArray(puncDamage.DamagesAI);
        }
    }
}
