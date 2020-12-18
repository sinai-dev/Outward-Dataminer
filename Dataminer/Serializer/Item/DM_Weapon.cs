using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_Weapon : DM_Equipment
    {
        public Weapon.WeaponType? WeaponType;
        public bool? Unblockable;
        public SwingSoundWeapon? SwingSound;
        public bool? SpecialIsZoom;
        public int? MaxProjectileShots;

        public float BaseHealthAbsorbRatio;
        public float BaseMaxHealthAbsorbRatio;

        public bool IgnoreHalfResistances;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var weapon = item as Weapon;

            WeaponType = weapon.Type;
            Unblockable = weapon.Unblockable;
            SwingSound = weapon.SwingSoundType;
            SpecialIsZoom = weapon.SpecialIsZoom;
            MaxProjectileShots = -1;

            BaseHealthAbsorbRatio = weapon.BaseHealthAbsorbRatio;
            BaseMaxHealthAbsorbRatio = weapon.BaseMaxHealthAbsorbRatio;

            IgnoreHalfResistances = weapon.IgnoreHalfResistances;

            if (weapon.GetComponent<WeaponLoadout>() is WeaponLoadout loadout)
            {
                MaxProjectileShots = loadout.MaxProjectileLoaded;
            }
        }
    }
}
