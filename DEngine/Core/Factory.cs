using System.Collections.Generic;

namespace DEngine.Core {   
//    abstract public class Factory<TKey, TBase> {
//        protected readonly Dictionary<TKey, TBase> products = new Dictionary<TKey, TBase>();
//
//        protected void Add(TKey key, TBase @base) {
//            products.Add(key, @base);
//        }
//
//        protected virtual TBase Create(TKey key) {
//            return products[key];
//        }
//
//    }

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
