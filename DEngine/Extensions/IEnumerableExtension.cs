using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Extensions {
    public static class EnumerableExtension {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var element in collection) {
                action(element);
            }

            return collection;
        }
    }
}
