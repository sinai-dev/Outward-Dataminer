using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_ItemStats
    {
        public int BaseValue;
        public float RawWeight;
        public int MaxDurability;

        public static DM_ItemStats ParseItemStats(ItemStats stats)
        {
            var type = Serializer.GetDMType(stats.GetType());

            var holder = (DM_ItemStats)Activator.CreateInstance(type);

            holder.SerializeStats(stats, holder);

            return holder;
        }
    
        public virtual void SerializeStats(ItemStats stats, DM_ItemStats holder)
        {
            holder.BaseValue = stats.BaseValue;
            holder.MaxDurability = stats.MaxDurability;
            holder.RawWeight = stats.RawWeight;
        }
    }
}
