using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using DEngine.Actor;

namespace DEngine.Entities {
	/// <summary>
	/// Default ComponentManager - maintains all loaded components and
	/// their associations with entities.  Basically a big Dictionary
	/// of dictionaries.
	/// </summary>
	internal class ComponentManager : IComponentManager {
		/// <summary>
		/// Collection of all components, separated by type.
		/// </summary>
		Dictionary<Type, Dictionary<UniqueId, Component>> components;

		private Dictionary<UniqueId, Component> this[Type t] {
			get {
				Contract.Ensures(Contract.Result<Dictionary<UniqueId, Component>>() != null);
				// Ensure that the manager has a dictionary ready for the component type
				if (!components.ContainsKey(t)) {
					// Check that our type is valid
					if (!typeof(Component).IsAssignableFrom(t))
						throw new ArgumentException("Type does not implement EntityComponent", "t");

					components.Add(t, new Dictionary<UniqueId, Component>());
				}

				return components[t];
			}
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(components != null);
		}


		internal ComponentManager() {
			components = new Dictionary<Type, Dictionary<UniqueId, Component>>();
		}

		/// <summary>
		/// Add a new component to an entity
		/// </summary>
		public void Add<T>(Entity e, T o)
				where T : Component {
			o.Entity = e;
			this[o.GetType()].Add(e.Id, o);
		}

		/// <summary>
		/// Add a collection of components to an entity
		/// </summary>
		public void Add(Entity e, IEnumerable<Component> comps) {
			foreach (var component in comps) {
				Add(e, component);
			}
		}

		/// <summary>
		/// Remove an entity from the manager
		/// </summary>
		/// <param name="e"></param>
		public void Remove(Entity e) {
			foreach (var c in components.Values) {
				c.Remove(e.Id);
			}
		}

		/// <summary>
		/// Remove a component from an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool Remove<T>(Entity e)
				where T : Component {
			return this[typeof(T)].Remove(e.Id);
		}

		/// <summary>
		/// Try to get a component from an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <returns></returns>
		public T Get<T>(Entity e) where T : Component {
			Component o;
			this[typeof(T)].TryGetValue(e.Id, out o);
			return (T)o;
		}

		/// <summary>
		/// Determine if the entity contains a component type
		/// </summary>
		/// <param name="e"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool Contains(Entity e, Type t) {
			return this[t].ContainsKey(e.Id);
		}

		#region IEnumerable

		public IEnumerator<Type> GetEnumerator() {
			return components.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		public IEnumerable<Component> All(UniqueId id) {
			var list = new List<Component>();

			foreach (var collection in components) {
				if (collection.Value.ContainsKey(id)) {
					list.Add(collection.Value[id]);
				}
			}

			return list;
		}
	}
}