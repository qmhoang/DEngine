using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace DEngine.Extensions {
	public class LambdaComparer<T> : IComparer<T> {
		readonly Comparison<T> comparer;

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(comparer != null);
		}

		public LambdaComparer(Comparison<T> comparer) {
			this.comparer = comparer;
		}

		public int Compare(T x, T y) {
			return comparer(x, y);
		}
	}
}