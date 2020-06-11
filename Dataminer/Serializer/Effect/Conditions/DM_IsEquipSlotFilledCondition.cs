using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_IsEquipSlotFilledCondition : DM_EffectCondition
    {
        public EquipmentSlot.EquipmentSlotIDs EquipmentSlot;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            (template as DM_IsEquipSlotFilledCondition).EquipmentSlot = (component as IsEquipSlotFilledCondition).EquipmentSlot;   
        }
    }
}
