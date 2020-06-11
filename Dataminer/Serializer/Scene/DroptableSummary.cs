using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class DroptableSummary
    {
        public string DropTableName;
        public List<string> Locations = new List<string>();
    }
}
