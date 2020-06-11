using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_Equipment : DM_Item
    {
        public EquipmentSlot.EquipmentSlotIDs? EquipSlot;
        public Equipment.TwoHandedType? TwoHandType;
        public Equipment.IKMode? IKType;

        public float? VisualDetectabilityAdd;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_Equipment;
            var equipment = item as Equipment;

            template.EquipSlot = equipment.EquipSlot;
            template.VisualDetectabilityAdd = equipment.VisualDetectabilityAdd;
            template.TwoHandType = equipment.TwoHand;
            template.IKType = equipment.IKType;
        }
    }
}
