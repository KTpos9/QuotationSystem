using System;
using System.Collections.Generic;

namespace Zero.Extension
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> items)
        {
            return items == null || !items.Any();
        }

        public static bool Any<T>(this ICollection<T> list)
        {
            return list.Count > 0;
        }

        public static void AddWhen<T>(this ICollection<T> list, bool condition, T item)
        {
            if (condition)
            {
                list.Add(item);
            }
        }

        public static void AddWhen<T>(this ICollection<T> list, bool condition, Func<T> getItem)
        {
            if (condition)
            {
                var item = getItem();
                list.Add(item);
            }
        }
    }
}