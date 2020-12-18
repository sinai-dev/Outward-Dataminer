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
        public bool Percentage;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            EquipmentSlot = (effect as ReduceDurability).EquipmentSlot;
            Durability = (effect as ReduceDurability).Durability;
            Percentage = (effect as ReduceDurability).Percentage;
        }
    }
}
