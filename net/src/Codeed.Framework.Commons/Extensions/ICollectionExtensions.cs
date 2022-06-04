using System.Collections.Generic;

namespace Codeed.Framework.Commons.Extensions
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Remove(item);
            }
        }
    }
}
