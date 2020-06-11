using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class DropTableEntry
    {
        public string Item_Name;
        public int Item_ID;

        public int Min_Quantity;
        public int Max_Quantity;
    }

    [DM_Serialized]
    public class DropTableChanceEntry : DropTableEntry
    {
        public float Drop_Chance;
        public int Dice_Range;
        public int ChanceReduction;
        public float ChanceRegenDelay;
        public float ChanceRegenQty;
    }
}
