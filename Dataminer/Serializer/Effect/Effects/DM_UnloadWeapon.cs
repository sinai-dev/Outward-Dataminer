using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_UnloadWeapon : DM_Effect
    {
        public Weapon.WeaponSlot WeaponSlot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_UnloadWeapon).WeaponSlot = (effect as UnloadWeapon).WeaponSlot;
        }
    }
}
