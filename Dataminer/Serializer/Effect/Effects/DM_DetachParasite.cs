using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_DetachParasite : DM_Effect
    {
        // This class uses no fields, it's a self-executing effect.

        public override void SerializeEffect<T>(T effect, DM_Effect holder) { }
    }
}
