using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_ImbueWeapon : DM_Effect
    {
        public float Lifespan;
        public int ImbueEffect_Preset_ID;
        public Weapon.WeaponSlot Imbue_Slot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_ImbueWeapon).ImbueEffect_Preset_ID = (effect as ImbueWeapon).ImbuedEffect.PresetID;
            (holder as DM_ImbueWeapon).Imbue_Slot = (effect as ImbueWeapon).AffectSlot;
            (holder as DM_ImbueWeapon).Lifespan = (effect as ImbueWeapon).LifespanImbue;
        }
    }
}
