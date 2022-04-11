using RelegatiaCCG.rccg.engine;
using System;
using System.Collections.Generic;

namespace mg.pummelz
{
    public static class MGPumBucketSorting
    {
      
        private static MGPumCRT getRarity(MGPumUnit unit)
        {
            return unit.rarity;
        }

        public static Dictionary<T, List<MGPumUnit>> sort<T>(IEnumerable<MGPumUnit> units, Func<MGPumUnit, T> getSortAttribute)
        {
            Dictionary<T, List<MGPumUnit>> result = new Dictionary<T, List<MGPumUnit>>();

            foreach (MGPumUnit unit in units)
            {
                if (!result.ContainsKey(getSortAttribute(unit)))
                {
                    result.Add(getSortAttribute(unit), new List<MGPumUnit>());
                }
                result[getSortAttribute(unit)].Add(unit);
            }

            return result;
        }

        public static Dictionary<MGPumCRT, List<MGPumUnit>> sortByRarity(IEnumerable<MGPumUnit> units)
        {
            return sort<MGPumCRT>(units, getRarity);
        }
    }
}

