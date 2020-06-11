using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ReduceDurability : DM_Effect
    {
        public float Durability;
        public EquipmentSlot.EquipmentSlotIDs EquipmentSlot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_ReduceDurability).EquipmentSlot = (effect as ReduceDurability).EquipmentSlot;
            (holder as DM_ReduceDurability).Durability = (effect as ReduceDurability).Durability;
        }
    }
}
