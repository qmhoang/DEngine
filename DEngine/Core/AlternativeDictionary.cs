﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace DEngine.Core {
	/// <summary>
	/// Represents a collection of default items that can be selectively overriden with
	/// alternatives.  When retrieving a value, the alternative is returned if one exists; otherwise
	/// the default is returned.<para/>
	/// </summary>
	/// <remarks>
	/// Conceptually, an AlternativeMap is similar to a Dictionary, albeit with a much simpler interface.
	/// An AlternativeMap is also a type of IStaticMap, so it can be used as the defaults for another
	/// AlternativeMap.
	/// <para/>
	/// All alternative items that are added must share a common key with one of the defaults, or an exception will
	/// be thrown.
	/// All AlternativeMap objects must have a valid collection of defaults, defined as an 
	/// IStaticDictionary object.
	/// The AlternateMap object stores a reference to the IStaticDictionary defaults, 
	/// so that changes to the default items elsewhere will affect the retrieval of items in this object.  
	/// It is for this reason that the defaults must be defined as an IStaticDictionary, so that 
	/// additions/removals to the default collection outside of this object do not introduce instability.
	/// </remarks>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class AlternativeMap<TKey, TValue> : IStaticDictionary<TKey, TValue> {
		/// <summary>
		/// Construct an AlternativeMap instance with the specified default items.  This class
		/// supports a collection initializer to add alternative items during construction.
		/// </summary>
		/// <param name="defaults">A valid, non-null StaticMap collection holding the default items</param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="defaults"/>
		/// is null</exception>
		public AlternativeMap(IStaticDictionary<TKey, TValue> defaults) {
			if (defaults == null)
				throw new ArgumentNullException("defaults");

			this._defaults = defaults;
			_alternatives = new Dictionary<TKey, TValue>();
		}

		public AlternativeMap(IStaticDictionary<TKey, TValue> defaults,
		                      Dictionary<TKey, TValue> alternatives) {
			if (defaults == null)
				throw new ArgumentNullException("defaults");

			if (alternatives == null)
				throw new ArgumentNullException("alternatives");

			this._defaults = defaults;
			this._alternatives = new Dictionary<TKey, TValue>();

			foreach (var itm in alternatives)
				this._alternatives.Add(itm.Key, itm.Value);
		}

		/// <summary>
		/// Adds the specified key and value to the alternatives.  The key must be defined
		/// in the StaticMap defaults passed during the constructor or an exception will
		/// be thrown.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
		/// is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="key"/>
		/// does not exist in the default items.</exception>
		public void Add(TKey key, TValue value) {
			if (key == null)
				throw new ArgumentNullException("key");
			if (!_defaults.ContainsKey(key))
				throw new ArgumentException("Specified key does not exist in default items");
			_alternatives.Add(key, value);
		}

		/// <summary>
		/// Removes the specified alternative so that the default will be retrieved.  If the
		/// alternative with the key exists, then this method returns true.  Otherwise this
		/// method returns false.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
		/// is null.</exception>
		public bool RemoveAlternative(TKey key) {
			if (key == null)
				throw new ArgumentNullException("key");
			return _alternatives.Remove(key);
		}

		/// <summary>
		/// Removes all of the alternatives, exposing the defaults for retrieval.
		/// </summary>
		public void RemoveAllAlternatives() {
			_alternatives.Clear();
		}

		/// <summary>
		/// Gets or sets the item with the specified key.  If retrieving a value, the alternative
		/// item will be returned if one exists, otherwise the default will be returned.  If setting
		/// a value, the alternative item is changed if the key exists; otherwise the alternative 
		/// item is added.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
		/// is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="key"/>
		/// does not exist in the default items.</exception>
		public TValue this[TKey key] {
			get {
				if (!_defaults.ContainsKey(key))
					throw new ArgumentException("Specified key does not exist");
				if (key == null)
					throw new ArgumentNullException("key");

				if (_alternatives.ContainsKey(key))
					return _alternatives[key];
				else
					return _defaults[key];
			}
			set {
				if (!_defaults.ContainsKey(key))
					throw new ArgumentException("Specified key does not exist");
				if (key == null)
					throw new ArgumentNullException("key");

				_alternatives[key] = value;
			}
		}

		public int Count {
			get { return _defaults.Count; }
		}

		public bool ContainsKey(TKey key) {
			return _defaults.ContainsKey(key);
		}

		public bool ContainsValue(TValue value) {
			if (_defaults.ContainsValue(value))
				return true;

			if (_alternatives.ContainsValue(value))
				return true;

			return false;
		}

		public Dictionary<TKey, TValue>.KeyCollection Keys {
			get { return _defaults.Keys; }
		}

		#region IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through this collection.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
			foreach (var v in _defaults)
				if (_alternatives.ContainsKey(v.Key))
					yield return new KeyValuePair<TKey, TValue>(v.Key, _alternatives[v.Key]);
				else
					yield return new KeyValuePair<TKey, TValue>(v.Key, v.Value);
		}

		/// <summary>
		/// Returns an enumerator that iterates through this collection.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		private readonly IStaticDictionary<TKey, TValue> _defaults;
		private readonly Dictionary<TKey, TValue> _alternatives;
	}
}