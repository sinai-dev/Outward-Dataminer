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

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var weapon = item as Weapon;
            var template = holder as DM_Weapon;

            template.WeaponType = weapon.Type;
            template.Unblockable = weapon.Unblockable;
            template.SwingSound = weapon.SwingSoundType;
            template.SpecialIsZoom = weapon.SpecialIsZoom;
            template.MaxProjectileShots = -1;

            if (weapon.GetComponent<WeaponLoadout>() is WeaponLoadout loadout)
            {
                template.MaxProjectileShots = loadout.MaxProjectileLoaded;
            }
        }
    }
}
