using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_TimeDelayCondition : DM_EffectCondition
    {
        public Vector2 DelayRange;
        public TimeDelayCondition.TimeType TimeFormat;
        public bool IgnoreFirstCheck = true;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var holder = template as DM_TimeDelayCondition;
            var comp = component as TimeDelayCondition;

            holder.DelayRange = comp.DelayRange;
            holder.TimeFormat = comp.TimeFormat;
            holder.IgnoreFirstCheck = comp.IgnoreFirstCheck;
        }
    }
}
