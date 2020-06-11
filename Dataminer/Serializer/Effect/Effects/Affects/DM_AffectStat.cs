using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AffectStat : DM_Effect
    {
        public string Stat_Tag = "";
        public float AffectQuantity;
        public bool IsModifier;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectStat).Stat_Tag = (effect as AffectStat).AffectedStat.Tag.TagName;
            (holder as DM_AffectStat).AffectQuantity = (effect as AffectStat).Value;
            (holder as DM_AffectStat).IsModifier = (effect as AffectStat).IsModifier;
        }
    }
}
