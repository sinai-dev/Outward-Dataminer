using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_PunctualDamage : DM_Effect
    {
        public List<DM_Damage> Damage = new List<DM_Damage>();
        public List<DM_Damage> Damages_AI = new List<DM_Damage>();
        public float Knockback;
        public bool HitInventory;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var puncDamage = effect as PunctualDamage;
            var puncHolder = holder as DM_PunctualDamage;

            puncHolder.Knockback = puncDamage.Knockback;
            puncHolder.HitInventory = puncDamage.HitInventory;
            puncHolder.Damage = DM_Damage.ParseDamageArray(puncDamage.Damages);
            puncHolder.Damages_AI = DM_Damage.ParseDamageArray(puncDamage.DamagesAI);
        }
    }
}
