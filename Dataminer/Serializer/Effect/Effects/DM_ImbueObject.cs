using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataminer 
{ 
    public class DM_ImbueObject : DM_Effect
    {
        public float Lifespan;
        public int ImbueEffect_Preset_ID;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as ImbueObject;

            ImbueEffect_Preset_ID = comp.ImbuedEffect?.PresetID ?? -1;
            Lifespan = comp.LifespanImbue;
        }
    }
}
