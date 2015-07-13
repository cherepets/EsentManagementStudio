using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EsentManagementStudio.Extensions
{
    public static class EnumerableExt
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
            => enumerable.ToList().ForEach(action);

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            var collection = new ObservableCollection<T>();
            enumerable.ForEach(i => collection.Add(i));
            return collection;
        }
    }
}
