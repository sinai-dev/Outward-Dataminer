using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_EquipDurabilityCondition : DM_EffectCondition
    {
        public EquipmentSlot.EquipmentSlotIDs EquipmentSlot;
        public float MinimumDurability;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_EquipDurabilityCondition).MinimumDurability = (component as EquipDurabilityCondition).DurabilityRequired;
            (template as DM_EquipDurabilityCondition).EquipmentSlot = (component as EquipDurabilityCondition).EquipmentSlot;
        }
    }
}
