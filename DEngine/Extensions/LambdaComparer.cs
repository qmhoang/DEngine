using System;
using System.Collections.Generic;

namespace DEngine.Extensions {
	public class LambdaComparer<T> : IComparer<T> {
		readonly Func<T, T, int> comparer;

		public LambdaComparer(Func<T, T, int> comparer) {
			this.comparer = comparer;
		}

		public int Compare(T x, T y) {
			return comparer(x, y);
		}
	}
}