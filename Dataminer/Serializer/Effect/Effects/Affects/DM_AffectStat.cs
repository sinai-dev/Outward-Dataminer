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
        public string[] Tags;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as AffectStat;
            this.Stat_Tag = comp.AffectedStat.Tag.TagName;
            this.AffectQuantity = comp.Value;
            this.IsModifier = comp.IsModifier;

            if (comp.Tags != null)
                this.Tags = comp.Tags
                           .Select(it => it.Tag.TagName)
                           .ToArray();
        }
    }
}
