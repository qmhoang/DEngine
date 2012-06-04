using System.Collections.Generic;

namespace DEngine.Utility {   
    abstract public class Factory<TKey, TBase> {
        protected readonly Dictionary<TKey, TBase> products = new Dictionary<TKey, TBase>();

        protected void Add(TKey key, TBase @base) {
            products.Add(key, @base);
        }

        protected virtual TBase Create(TKey key) {
            return products[key];
        }

    }
}
