using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_RemoveImbueEffects : DM_Effect
    {
        public Weapon.WeaponSlot AffectSlot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_RemoveImbueEffects).AffectSlot = (effect as RemoveImbueEffects).AffectSlot;
        }
    }
}
