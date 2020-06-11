using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_RunicBlade : DM_Effect
    {
        public int WeaponID;
        public int GreaterWeaponID;

        public int PrefixImbueID;
        public int PrefixGreaterImbueID;

        public float SummonLifespan;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var template = holder as DM_RunicBlade;
            var comp = effect as RunicBlade;

            template.SummonLifespan = comp.SummonLifeSpan;
            template.WeaponID = comp.RunicBladePrefab.ItemID;
            template.GreaterWeaponID = comp.RunicGreatBladePrefab.ItemID;
            template.PrefixImbueID = comp.ImbueAmplifierRunicBlade.PresetID;
            template.PrefixGreaterImbueID = comp.ImbueAmplifierGreatRunicBlade.PresetID;
        }
    }
}
