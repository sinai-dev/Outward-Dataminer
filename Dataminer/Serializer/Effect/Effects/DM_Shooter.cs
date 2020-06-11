using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace Dataminer
{
    /// <summary>
    /// Abstract base class for DM_ShootBlast and DM_ShootProjectile
    /// </summary>
    public abstract class DM_Shooter : DM_Effect
    {
        public Shooter.CastPositionType CastPosition;
        public Vector3 LocalPositionAdd;
        public bool NoAim;
        public Shooter.TargetTypes TargetType;
        public string TransformName;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var comp = effect as Shooter;
            var template = holder as DM_Shooter;

            template.CastPosition = comp.CastPosition;
            template.NoAim = comp.NoAim;
            template.LocalPositionAdd = comp.LocalCastPositionAdd;
            template.TargetType = comp.TargetType;
            template.TransformName = comp.TransformName;
        }
    }
}
