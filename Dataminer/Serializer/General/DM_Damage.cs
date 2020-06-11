using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Damage
    {
        public float Damage = 0f;
        public DamageType.Types Type = DamageType.Types.Count;

        public static DamageList GetDamageList(List<DM_Damage> list)
        {
            var newlist = new DamageList();
            foreach (var entry in list)
            {
                newlist.Add(entry.GetDamageType());
            }

            return newlist;
        }

        public DamageType GetDamageType()
        {
            return new DamageType(this.Type, this.Damage);
        }

        // --------- from game class to our holder ----------

        public static List<DM_Damage> ParseDamageList(DamageList list)
        {
            var damages = new List<DM_Damage>();

            foreach (DamageType type in list.List)
            {
                damages.Add(ParseDamageType(type));
            }

            return damages;
        }

        public static List<DM_Damage> ParseDamageArray(DamageType[] types)
        {
            List<DM_Damage> damages = new List<DM_Damage>();

            foreach (DamageType type in types)
            {
                damages.Add(ParseDamageType(type));
            }

            return damages;
        }

        public static DM_Damage ParseDamageType(DamageType damage)
        {
            return new DM_Damage
            {
                Damage = damage.Damage,
                Type = damage.Type
            };
        }
    }
}
