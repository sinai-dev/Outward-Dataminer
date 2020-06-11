using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_Summon : DM_Effect
    {
        public const string CorruptionSpiritPath = @"CorruptionSpirit.prefab";
        public const string SummonGhostPath = @"NewGhostOneHandedAlly.prefab";

        public enum PrefabTypes
        {
            Item,
            Resource
        }

        // if Item: the ItemID, if Character: the 'Resources/' asset path.
        public string Prefab;
        public PrefabTypes SummonPrefabType;
        public int BufferSize = 1;
        public bool LimitOfOne;
        public Summon.InstantiationManagement SummonMode;
        public Summon.SummonPositionTypes PositionType;
        public float MinDistance;
        public float MaxDistance;
        public bool SameDirectionAsSummoner;
        public Vector3 SummonLocalForward;
        public bool IgnoreOnDestroy;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var template = holder as DM_Summon;
            var comp = effect as Summon;

            if (effect is SummonAI)
            {
                template.SummonPrefabType = PrefabTypes.Resource;

                var name = comp.SummonedPrefab.name.Replace("(Clone)", "").Trim();

                template.Prefab = name;
            }
            else
            {
                template.SummonPrefabType = PrefabTypes.Item;

                template.Prefab = comp.SummonedPrefab.gameObject.GetComponent<Item>().ItemID.ToString();
            }

            template.BufferSize = comp.BufferSize;
            template.LimitOfOne = comp.LimitOfOne;
            template.SummonMode = comp.InstantiationMode;
            template.PositionType = comp.PositionType;
            template.MinDistance = comp.MinDistance;
            template.MaxDistance = comp.MaxDistance;
            template.SameDirectionAsSummoner = comp.SameDirAsSummoner;
            template.SummonLocalForward = comp.SummonLocalForward;
            template.IgnoreOnDestroy = comp.IgnoreOnDestroy;
        }
    }
}
