using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_AngleFwdToTargetFwdCondition : DM_EffectCondition
    {
        public List<Vector2> AnglesToCompare = new List<Vector2>();

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var holder = template as DM_AngleFwdToTargetFwdCondition;
            var angles = (component as AngleFwdToTargetFwdCondition).AnglesToCompare;

            holder.AnglesToCompare = angles.ToList();
        }
    }
}
