using System;
using System.Collections.Generic;
using DEngine.Actor;

namespace DEngine.Entity {
	/// <summary>
	/// Interface for a ComponentManager.  Handles storage and retrieval of component
	/// types associated with entity ids.
	/// </summary>
	public interface IComponentManager : IEnumerable<Type> {
		/// <summary>
		/// Add a new component to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entityId"></param>
		/// <param name="o"></param>
		void Add<T>(UniqueId entityId, T o) where T : EntityComponent;

		/// <summary>
		/// Add a list of components to an entity
		/// </summary>
		/// <param name="id"></param>
		/// <param name="components"></param>
		void Add(UniqueId id, IEnumerable<EntityComponent> components);

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
		bool Remove<T>(UniqueId id) where T : EntityComponent;

		/// <summary>
		/// Get a component beloning to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		T Get<T>(UniqueId id) where T : EntityComponent;

		/// <summary>
		/// Check if an entity has a component type
		/// </summary>
		/// <param name="id"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		bool Contains(UniqueId id, Type t);

		IEnumerable<EntityComponent> All(UniqueId id);
	}
}