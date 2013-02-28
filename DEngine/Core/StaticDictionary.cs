using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.Contracts;

namespace DEngine.Core {
	/// <summary>
	/// Represents a type of Dictionary that, after construction, has a static number of
	/// items.  Items cannot be added or removed, but they can be modified as normal.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[ContractClass(typeof(StaticDictionaryContract))]
	public interface IStaticDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> {
		/// <summary>
		/// Get or set the value of the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		TValue this[TKey key] { get; set; }

		/// <summary>
		/// Gets the number of items in this collection.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets a list of keys contained in this collection.
		/// </summary>
		Dictionary<TKey, TValue>.KeyCollection Keys { get; }

		/// <summary>
		/// Returns true if the specified key is contained in this collection.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[Pure]
		bool ContainsKey(TKey key);

		/// <summary>
		/// Returns true if the specified value is contained in this collection.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[Pure]
		bool ContainsValue(TValue value);
	}

	[ContractClassFor(typeof(IStaticDictionary<object, object>))]
	abstract class StaticDictionaryContract : IStaticDictionary<object, object> {
		public object this[object key] {
			get {
				Contract.Requires<ArgumentNullException>(key != null, "key");
				Contract.Requires<ArgumentException>(ContainsKey(key), "Specified key does not exist");

				return default(object);
			}

			set {
				Contract.Requires<ArgumentNullException>(key != null, "key");
				Contract.Requires<ArgumentException>(ContainsKey(key), "Specified key does not exist");
			}
		}

		public int Count {
			get { 
				Contract.Ensures(Contract.Result<int>() >= 0);
				return default(int);
			}
		}

		public bool ContainsKey(object key) {
			Contract.Requires<ArgumentNullException>(key != null, "key");
			return default(bool);
		}

		public abstract Dictionary<object, object>.KeyCollection Keys { get; }
		public abstract bool ContainsValue(object value);
		public abstract IEnumerator<KeyValuePair<object, object>> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}


	/// <summary>
	/// Represents an IStaticMap object that is constructed using an array
	/// of key-value pairs.  Once constructed, items cannot be added or removed.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class StaticDictionary<TKey, TValue> : IStaticDictionary<TKey, TValue> {
		/// <summary>
		/// Construct a StaticDictionary instance given an array of key value pairs.
		/// </summary>
		/// <param name="items"></param>
		public StaticDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items) {
			dictionary = new Dictionary<TKey, TValue>();

			foreach (var itm in items)
				dictionary.Add(itm.Key, itm.Value);
		}

		/// <summary>
		/// Gets or sets the value with the specified key.  The key must be non-null, and must
		/// exist in this StaticDictionary or an exception will be thrown.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
		/// is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="key"/>
		/// does not exist in the default items.</exception>
		public TValue this[TKey key] {
			get {
				return dictionary[key];
			}
			set {
				dictionary[key] = value;
			}
		}

		/// <summary>
		/// Gets the number of items contained in this StaticDictionary
		/// </summary>
		public int Count {
			get { return dictionary.Count; }
		}

		/// <summary>
		/// Gets a collection containing the keys.
		/// </summary>
		public Dictionary<TKey, TValue>.KeyCollection Keys {
			get { return dictionary.Keys; }
		}

		/// <summary>
		/// Gets a collection containing the values.
		/// </summary>
		public Dictionary<TKey, TValue>.ValueCollection Values {
			get { return dictionary.Values; }
		}

		/// <summary>
		/// Returns true if this StaticDictionary contains the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
		/// is null.</exception>
		public bool ContainsKey(TKey key) {
			return dictionary.ContainsKey(key);
		}

		/// <summary>
		/// Returns true if this StaticDictionary contains the specified value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool ContainsValue(TValue value) {
			return dictionary.ContainsValue(value);
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return dictionary.ToString();
		}

		/// <summary>
		/// Returns an enumerator that iterates through this collection.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
			return dictionary.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through this collection.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		private Dictionary<TKey, TValue> dictionary;

	}


}