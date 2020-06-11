using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

namespace Dataminer
{
    [DM_Serialized]
    public abstract class DM_Effect
    {
        public float Delay = 0f;
        public Effect.SyncTypes SyncType = Effect.SyncTypes.Everyone;
        public EffectSynchronizer.EffectCategories OverrideCategory = EffectSynchronizer.EffectCategories.None;

        public abstract void SerializeEffect<T>(T effect, DM_Effect holder) where T : Effect;

        public static DM_Effect ParseEffect(Effect effect)
        {
            var type = effect.GetType();

            if (Serializer.GetBestDMType(type) is Type dm_type && !dm_type.IsAbstract)
            {
                var holder = Activator.CreateInstance(dm_type) as DM_Effect;
                holder.Delay = effect.Delay;
                holder.OverrideCategory = effect.OverrideEffectCategory;
                holder.SyncType = effect.SyncType;

                holder.SerializeEffect(effect, holder);
                return holder;
            }
            else
            {
                Debug.Log(type + " is not supported yet!");
                return null;
            }
        }
    }
}
