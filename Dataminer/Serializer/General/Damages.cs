using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    [DM_Serialized]
    public class Damages
    {
        public float Damage = 0f;
        public DamageType.Types Damage_Type = DamageType.Types.Count;

        public static DamageList GetDamageList(List<Damages> list)
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
            return new DamageType(this.Damage_Type, this.Damage);
        }

        // --------- from game class to our holder ----------

        public static List<Damages> ParseDamageList(DamageList list)
        {
            var damages = new List<Damages>();

            foreach (DamageType type in list.List)
            {
                damages.Add(ParseDamageType(type));
            }

            return damages;
        }

        public static List<Damages> ParseDamageArray(DamageType[] types)
        {
            List<Damages> damages = new List<Damages>();

            foreach (DamageType type in types)
            {
                damages.Add(ParseDamageType(type));
            }

            return damages;
        }

        public static Damages ParseDamageType(DamageType damage)
        {
            return new Damages
            {
                Damage = damage.Damage,
                Damage_Type = damage.Type
            };
        }
    }
}
