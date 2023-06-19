using System;
using System.Collections.Generic;
using System.Linq;

namespace Zero.Extension
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            return source;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keyer)
        {
            var keys = new HashSet<TKey>();
            var result = new List<T>();
            foreach (var item in source)
            {
                var key = keyer(item);
                if (keys.Contains(key))
                    continue;

                result.Add(item);
                keys.Add(key);
            }
            return result;
        }

        public static Dictionary<int, List<T>> GroupRows<T>(this List<T> source, int rowsPerGroup)
        {
            int i = 0;
            return source.Select(x => new { GroupId = (i++ / rowsPerGroup), Data = x })
                .GroupBy(x => x.GroupId)
                .Select(x => new { GroupId = x.Key, Result = x.Select(s => s.Data).ToList() })
                .ToDictionary(x => x.GroupId, x => x.Result);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> list)
        {
            var r = new Random();
            return list.OrderBy(x => r.Next());
        }

        public static List<DateTime> GetDateSequence(this DateTime startDate, DateTime endDate)
        {
            var period = new List<DateTime>();
            var temp = startDate;
            while (temp <= endDate)
            {
                period.Add(temp);
                temp = temp.AddDays(1);
            }

            return period;
        }
    }
}