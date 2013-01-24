using System;
using System.Collections.Generic;
using DEngine.Actor;

namespace DEngine.Entities {
	/// <summary>
	/// Interface for a ComponentManager.  Handles storage and retrieval of component
	/// types associated with entity ids.
	/// </summary>
	public interface IComponentManager : IEnumerable<Type> {
		/// <summary>
		/// Add a new component to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <param name="o"></param>
		void Add<T>(Entity entity, T o) where T : Component;

		/// <summary>
		/// Add a list of components to an entity
		/// </summary>
		/// <param name="e"></param>
		/// <param name="components"></param>
		void Add(Entity e, IEnumerable<Component> components);

		/// <summary>
		/// Remove all components belonging to an entity
		/// </summary>
		/// <param name="id"></param>
		void Remove(UniqueId id);

		/// <summary>
		/// Remove a component belonging to an entity
		/// </summary>
		/// <param name="id"></param>		
		/// <returns>If a component was removed or not</returns>
		bool Remove<T>(UniqueId id) where T : Component;

		/// <summary>
		/// Get a component beloning to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		T Get<T>(UniqueId id) where T : Component;

		/// <summary>
		/// Check if an entity has a component type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		bool Contains(UniqueId id, Type t);

		IEnumerable<Component> All(UniqueId id);
	}
}