using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dataminer
{
    public class DM_Instrument : DM_Item
    {
        public float PeriodicTime;
        public Vector2 PulseSpeed;
        public float StrikeTime;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var comp = item as Instrument;

            PeriodicTime = comp.PeriodicTime;
            PulseSpeed = comp.PulseSpeed;
            StrikeTime = comp.StrikeTime;
        }
    }
}
