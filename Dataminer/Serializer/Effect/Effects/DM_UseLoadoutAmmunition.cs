using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_UseLoadoutAmmunition : DM_Effect
    {
        public bool MainHand;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_UseLoadoutAmmunition).MainHand = (effect as UseLoadoutAmunition).MainHand;
        }
    }
}
