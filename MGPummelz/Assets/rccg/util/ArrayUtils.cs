using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RelegatiaCCG.rccg.util
{
    public static class ArrayUtils
    {
        
        public static T[] shuffle<T>(this T[] array)
        {
            var rng = new Random();
            return array.Select(i => new { i, r = rng.Next() })
                                            .OrderBy(i => i.r)
                                            .Select(i => i.i)
                                            .ToArray();
        }

        public static T[] toShuffledArray<T>(this IEnumerable<T> enumerable)
        {
            var rng = new Random();
            T[] result = new T[enumerable.Count()];
            int i = 0;
            foreach(T t in enumerable)
            {
                result[i] = t;
                i++;
            }

            return result.shuffle();
        }

        public static IEnumerable<T> iterateReverse<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                yield return list[i];
            }
        }


    }
}

