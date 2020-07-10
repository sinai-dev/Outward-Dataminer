using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ImbueEffectORCondition : DM_EffectCondition
    {
        public List<int> ImbuePresetIDs;
        public bool AnyImbue;
        public Weapon.WeaponSlot WeaponToCheck;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as ImbueEffectORCondition;
            var holder = template as DM_ImbueEffectORCondition;

            holder.AnyImbue = comp.AnyImbue;
            holder.WeaponToCheck = comp.WeaponToCheck;

            if (comp.ImbueEffectPresets != null)
            {
                holder.ImbuePresetIDs = new List<int>();
                foreach (var imbue in comp.ImbueEffectPresets)
                {
                    holder.ImbuePresetIDs.Add(imbue.PresetID);
                }
            }
        }
    }
}
