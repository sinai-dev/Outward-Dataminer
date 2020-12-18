using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dataminer
{
    public class DM_WeaponDamageTargetHealth : DM_WeaponDamage
    {
        public Vector2 MultiplierHighLowHP;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            MultiplierHighLowHP = (effect as WeaponDamageTargetHealth).MultiplierHighLowHP;
        }
    }
}
