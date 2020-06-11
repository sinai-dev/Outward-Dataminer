using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_WeaponIsLoadedCondition : DM_EffectCondition
    {
        public Weapon.WeaponSlot SlotToCheck;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_WeaponIsLoadedCondition).SlotToCheck = (component as WeaponIsLoadedCondition).SlotToCheck;
        }
    }
}
