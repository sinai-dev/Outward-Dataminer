using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_LightLantern : DM_Effect
    {
        public bool Light;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_LightLantern).Light = (effect as LightLantern).Light;
        }
    }
}
