using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_Teleport : DM_Effect
    {
        public float MaxRange;
        public float MaxTargetRange;
        public float MaxYDiff;
        public Vector3 OffsetRelativeTarget;
        public bool UseTarget;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var template = holder as DM_Teleport;
            var comp = effect as Teleport;

            template.MaxRange = comp.MaxRange;
            template.MaxYDiff = comp.MaxYDiff;
            template.MaxTargetRange = comp.MaxTargetRange;
            template.OffsetRelativeTarget = comp.OffsetRelativeTarget;
            template.UseTarget = comp.UseTarget;
        }
    }
}
