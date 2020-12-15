using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;
using UnityEngine;

namespace Dataminer
{
    public class DM_Bag : DM_Equipment
    {
        public float? Capacity;
        public bool? Restrict_Dodge;
        public float? InventoryProtection;

        public float? Preserver_Amount = -1;
        public bool? Nullify_Perish = false;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var bag = item as Bag;
            var template = holder as DM_Bag;

            template.Capacity = bag.BagCapacity;
            template.Restrict_Dodge = bag.RestrictDodge;
            template.InventoryProtection = bag.InventoryProtection;

            if (bag.GetComponentInChildren<Preserver>() is Preserver p
                && At.GetField(p, "m_preservedElements") is List<Preserver.PreservedElement> list 
                && list.Count > 0)
            {
                template.Preserver_Amount = list[0].Preservation;
                template.Nullify_Perish = p.NullifyPerishing;
            }
        }
    }
}
