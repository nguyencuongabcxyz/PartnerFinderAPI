using System;
using System.Collections.Generic;

namespace PartnerFinder.Extensions
{
    public static class CollectionExtension
    {
        public static T GetRandomElement<T> (this List<T> collection)
        {
            var count = collection.Count;
            var randomIndex = new Random().Next(0, count - 1);
            return collection[randomIndex];
        }
    }
}
