using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_AddStatusImmunity : DM_Effect
    {
        public string ImmunityTag;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var selector = (TagSourceSelector)At.GetValue(typeof(AddStatusImmunity), effect, "m_statusImmunity");
            (holder as DM_AddStatusImmunity).ImmunityTag = selector.Tag.TagName;
        }
    }
}
