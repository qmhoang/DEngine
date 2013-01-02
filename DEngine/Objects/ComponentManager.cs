using System;
using System.Collections;
using System.Collections.Generic;
using DEngine.Actor;

namespace DEngine.Entity {
	/// <summary>
	/// Default ComponentManager - maintains all loaded components and
	/// their associations with entities.  Basically a big Dictionary
	/// of dictionaries.
	/// </summary>
	internal class ComponentManager : IComponentManager {
		/// <summary>
		/// Collection of all components, separated by type.
		/// </summary>
		Dictionary<Type, Dictionary<UniqueId, EntityComponent>> components;


		private Dictionary<UniqueId, EntityComponent> this[Type t] {
			get {
				// Ensure that the manager has a dictionary ready for the component type
				if (!components.ContainsKey(t)) {
					// Check that our type is valid
					if (!typeof(EntityComponent).IsAssignableFrom(t))
						throw new ArgumentException("Type does not implement EntityComponent", "t");

					components.Add(t, new Dictionary<UniqueId, EntityComponent>());
				}

				return components[t];
			}
		}


		internal ComponentManager() {
			components = new Dictionary<Type, Dictionary<UniqueId, EntityComponent>>();
		}

		/// <summary>
		/// Add a new component to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <param name="o"></param>
		public void Add<T>(UniqueId id, T o)
				where T : EntityComponent {
			o.Owner = id;   // Ensure the component is owned by the entity
			this[o.GetType()].Add(id, o);
		}

		/// <summary>
		/// Add a collection of components to an entity
		/// </summary>
		/// <param name="id"></param>
		/// <param name="components"></param>
		public void Add(UniqueId id, IEnumerable<EntityComponent> components) {
			foreach (var component in components) {
				Add(id, component);
			}
		}

		/// <summary>
		/// Remove an entity from the manager
		/// </summary>
		/// <param name="id"></param>
		public void Remove(UniqueId id) {
			foreach (var c in components.Values) {
				c.Remove(id);
			}
		}

		/// <summary>
		/// Remove a component from an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Remove<T>(UniqueId id)
				where T : EntityComponent {
			return this[typeof(T)].Remove(id);
		}

		/// <summary>
		/// Try to get a component from an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public T Get<T>(UniqueId id) where T : EntityComponent {
			EntityComponent o;
			this[typeof(T)].TryGetValue(id, out o);
			return (T)o;
		}

		/// <summary>
		/// Determine if the entity contains a component type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool Contains(UniqueId id, Type t) {
			return this[t].ContainsKey(id);
		}

		#region IEnumerable

		public IEnumerator<Type> GetEnumerator() {
			return components.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		public IEnumerable<EntityComponent> All(UniqueId id) {
			var list = new List<EntityComponent>();

			foreach (var collection in components) {
				if (collection.Value.ContainsKey(id)) {
					list.Add(collection.Value[id]);
				}
			}

			return list;
		}
	}
}