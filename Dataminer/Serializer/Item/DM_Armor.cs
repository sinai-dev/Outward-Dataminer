using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_Armor : DM_Equipment
    {
        public Armor.ArmorClass? Class;
        public EquipmentSoundMaterials? GearSoundMaterial;
        public EquipmentSoundMaterials? ImpactSoundMaterial;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_Armor;
            var armor = item as Armor;

            template.Class = armor.Class;
            template.GearSoundMaterial = armor.GearSoundMaterial;
            template.ImpactSoundMaterial = armor.ImpactSoundMaterial;
        }
    }
}
