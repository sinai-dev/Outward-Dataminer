﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;

namespace Dataminer
{
    public class DM_AffectFood : DM_Effect
    {
        public float AffectQuantity;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            (holder as DM_AffectFood).AffectQuantity = (float)At.GetField(effect as AffectNeed, "m_affectQuantity");
        }
    }
}
