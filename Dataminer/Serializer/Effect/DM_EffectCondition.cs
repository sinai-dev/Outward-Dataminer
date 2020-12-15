using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapMagic;
using SideLoader;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public abstract class DM_EffectCondition
    {
        public bool Invert;

        public static DM_EffectCondition ParseCondition(EffectCondition component)
        {
            var type = component.GetType();

            if (Serializer.GetDMType(type) is Type DM_type)
            {
                var holder = Activator.CreateInstance(DM_type) as DM_EffectCondition;

                holder.Invert = component.Invert;

                holder.SerializeEffect(component, holder);
                return holder;
            }
            else
            {
                SL.Log(type + " is not supported yet, sorry!");
                return null;
            }
        }

        public abstract void SerializeEffect<T>(EffectCondition component, T template) where T : DM_EffectCondition;
    }
}
