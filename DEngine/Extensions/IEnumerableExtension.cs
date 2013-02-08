using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DEngine.Extensions {
	public static class EnumerableExtension {
		public static IEnumerable<T> Each<T>(this IEnumerable<T> collection, Action<T> action) {
			Contract.Requires<ArgumentNullException>(action != null, "action");
			Contract.Requires<ArgumentNullException>(collection != null, "collection");
			foreach (var element in collection)
				action(element);
			return collection;
		}
	}
}