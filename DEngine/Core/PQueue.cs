using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public class PQueue<T> {
		private readonly SortedSet<T> _set;

		public PQueue() {
			_set = new SortedSet<T>();
		}

		public PQueue(IComparer<T> comparer) {
			_set = new SortedSet<T>(comparer);
		}

		public int Count { get { return _set.Count; } }

		public void Enqueue(T item) {
			_set.Add(item);
		}

		public T Dequeue() {
			var item = _set.Max;
			_set.Remove(item);
			return item;
		}
	}
}
