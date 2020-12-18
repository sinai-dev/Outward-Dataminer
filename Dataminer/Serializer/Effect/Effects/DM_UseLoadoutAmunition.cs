using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_UseLoadoutAmunition : DM_Effect
    {
        public bool MainHand;
        public bool AutoLoad;
        public bool DestroyOnEmpty;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            MainHand = (effect as UseLoadoutAmunition).MainHand;
            AutoLoad = (effect as UseLoadoutAmunition).AutoLoad;
            DestroyOnEmpty = (effect as UseLoadoutAmunition).DestroyOnEmpty;
        }
    }
}
