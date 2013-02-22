using System;
using System.Collections.Generic;

namespace DEngine.Extensions {
	public class NoSuchElementException : Exception {
		public NoSuchElementException() { }
		public NoSuchElementException(string message) : base(message) { }
		public NoSuchElementException(string message, Exception innerException) : base(message, innerException) { }
	}

	/// <summary>
	/// Java-style read only iterator
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Iterator<T> {
		private readonly IEnumerator<T> enumerator;

		public Iterator(IEnumerator<T> enumerator) {
			this.enumerator = enumerator;
			HasNext = enumerator.MoveNext();
			Current = enumerator.Current;
		}

		/// <summary>
		/// Returns true if the iterator has more elements.
		/// </summary>
		public bool HasNext { get; private set; }

		/// <summary>
		/// Returns the current item the iterator is pointed at
		/// </summary>
		public T Current { get; private set; }

		/// <summary>
		/// Move the iterator to the next element, returning the element it previously pointed to.
		/// </summary>
		/// <returns>the element it previously pointed to.</returns>
		/// <exception cref=""></exception>
		public T Next() {
			var prev = Current;
			HasNext = enumerator.MoveNext();
			Current = enumerator.Current;

			if (!HasNext)
				throw new NoSuchElementException();

			return prev;
		}
	}
}