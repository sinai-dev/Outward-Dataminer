using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{ 
    [DM_Serialized]
    public class DM_Gatherable
    {
        public string Name;
        public int ItemID;

        public List<string> DropTables = new List<string>();

        public static DM_Gatherable ParseGatherable(Gatherable gatherable)
        {
            var gatherableHolder = new DM_Gatherable
            {
                Name = gatherable.Name,
                ItemID = gatherable.ItemID
            };

            if (At.GetValue(typeof(SelfFilledItemContainer), gatherable, "m_drops") is List<Dropable> droppers)
            {
                if (droppers == null || droppers.Count < 1)
                {
                    //Debug.LogWarning("droppers is null or list count is 0!");
                }
                else
                {
                    foreach (Dropable dropper in droppers)
                    {
                        var dropableHolder = DM_DropTable.ParseDropTable(dropper);
                        gatherableHolder.DropTables.Add(dropableHolder.Name);
                    }
                }
            }

            if (gatherableHolder.Name == "Fish")
            {
                gatherableHolder.Name = "Fishing Spot (" + gatherableHolder.DropTables[0] + ")";
            }

            return gatherableHolder;
        }
    }
}
