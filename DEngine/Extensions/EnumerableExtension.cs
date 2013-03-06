using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Extensions {
	public static class EnumerableExtension {
		public static bool IsEmpty(this IEnumerable collection) {
			return !collection.GetEnumerator().MoveNext();
		}

		public static string GetEnumeratedString<T>(this IEnumerable<T> collection) {
			return collection.Aggregate("", (current, s) => current + s + ", ", s => s.Substring(0, s.Length - 2));
		}

		public static string GetEnumeratedString<T>(this IEnumerable<T> collection, Func<T, string> descriptor) {
			return collection.Aggregate("", (current, s) => current + descriptor(s) + ", ", s => s.Substring(0, s.Length - 2));
		}

		public static Iterator<T> GetIterator<T>(this IEnumerable<T> collection) {
			return new Iterator<T>(collection.GetEnumerator());
		}

		public static IEnumerable<T> Each<T>(this IEnumerable<T> collection, Action<T> action) {
			Contract.Requires<ArgumentNullException>(action != null, "action");
			Contract.Requires<ArgumentNullException>(collection != null, "collection");
			foreach (var element in collection)
				action(element);
			return collection;
		}

		public static IEnumerable<Entity> FilteredBy<T1>(this IEnumerable<Entity> collection) where T1 : Component {
			Contract.Requires<ArgumentNullException>(collection != null, "collection");
			foreach (Entity e in collection) {
				if (e.Has<T1>())
					yield return e;
			}
		}

		public static IEnumerable<Entity> FilteredBy<T1, T2>(this IEnumerable<Entity> collection)
			where T1 : Component
			where T2 : Component {
			Contract.Requires<ArgumentNullException>(collection != null, "collection");
			foreach (Entity e in collection) {
				if (e.Has<T1>() && e.Has<T2>())
					yield return e;
			}
		}

		public static IEnumerable<Entity> FilteredBy<T1, T2, T3>(this IEnumerable<Entity> collection)
			where T1 : Component
			where T2 : Component
			where T3 : Component {
			Contract.Requires<ArgumentNullException>(collection != null, "collection");
			foreach (Entity e in collection) {
				if (e.Has<T1>() && e.Has<T2>() && e.Has<T3>())
					yield return e;
			}
		}

		public static IEnumerable<Entity> FilteredBy(this IEnumerable<Entity> collection, params Type[] types) {
			Contract.Requires<ArgumentNullException>(collection != null, "collection");
			foreach (Entity e in collection) {
				if (types.Contains(e.GetType()))
					yield return e;
			}
		}
	}
}