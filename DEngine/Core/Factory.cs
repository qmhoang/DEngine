using System.Collections.Generic;

namespace DEngine.Core {
	public abstract class Factory<TProduct> {
		public abstract TProduct Construct();
	}

	public abstract class Factory<TKey, TProduct> {
		public abstract TProduct Construct(TKey identifier);
	}

	public abstract class Factory<TKey, TIdentifier, TProduct> {
		public abstract TProduct Construct(TKey key, TIdentifier uid);
	}
}