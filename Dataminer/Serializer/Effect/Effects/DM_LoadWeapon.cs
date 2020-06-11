using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_LoadWeapon : DM_Effect
    {
        public bool UnloadFirst;
        public Weapon.WeaponSlot WeaponSlot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_LoadWeapon).WeaponSlot = (effect as LoadWeapon).WeaponSlot;
            (holder as DM_LoadWeapon).UnloadFirst = (effect as LoadWeapon).UnloadFirst;
        }
    }
}
