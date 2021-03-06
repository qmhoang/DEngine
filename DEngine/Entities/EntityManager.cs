﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using DEngine.Actor;
using DEngine.Extensions;

namespace DEngine.Entities {

	#region Event Delegates

	/// <summary>
	/// Entity event delegate
	/// </summary>
	public delegate void EntityEventHandler(Entity entity);

	/// <summary>
	/// Entity id event delegate
	/// </summary>
	/// <param name="id"></param>
	public delegate void EntityIdEventHandler(UniqueId id);

	#endregion

	public sealed class EntityManager : IEnumerable<Entity> {
		/// <summary>
		/// All loaded entities
		/// </summary>
		internal readonly Dictionary<UniqueId, Entity> Entities;

		/// <summary>
		/// Filtered entity collections
		/// </summary>
		readonly Dictionary<int, FilteredCollection> _filteredCollections;

		/// <summary>
		/// Store of all loaded components, indexed by type and entity id
		/// </summary>
		readonly IComponentManager _components;

		/// <summary>
		/// Store of all loaded components, indexed by type and entity id
		/// </summary>
		internal IComponentManager Components {
			get {
				return _components;
			}
		}

		/// <summary>
		/// Filtered Entity Collections
		/// </summary>
		internal IEnumerable<FilteredCollection> FilteredCollections {
			get {
				return _filteredCollections.Values;
			}
		}

		/// <summary>
		/// Indexer by Entity Id
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Entity this[UniqueId index] {
			get {
				return Entities[index];
			}
		}

		/// <summary>
		/// Event for EntitySystems to run on adding an entity
		/// </summary>
		public event EntityEventHandler EntityAdded;

		private void OnEntityAdded(Entity e) {
			var handler = EntityAdded;
			if (handler != null)
				handler(e);
		}

		/// <summary>
		/// Event for EntitySystems to run on removal of an entity
		/// </summary>
		public event EntityEventHandler EntityRemoved;

		private void OnEntityRemoved(Entity e) {
			var handler = EntityRemoved;
			if (handler != null)
				handler(e);
		}

		#region Constructors

		public EntityManager()
			: this(new ComponentManager()) { }

		public EntityManager(IComponentManager componentManager) {
			Entities = new Dictionary<UniqueId, Entity>();
			_filteredCollections = new Dictionary<int, FilteredCollection>();
			_components = componentManager;
		}

		#endregion

		#region Get FilteredCollections

		/// <summary>
		/// Return all entities containing all of the parameter type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get<T>() where T : Component {
			return Get(new[] { typeof(T) });
		}

		/// <summary>
		/// Return all entities containing all of the parameter type T with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get<T>(IComparer<Entity> comparer) where T : Component {
			return Get(comparer, new[] { typeof(T) });
		}

		/// <summary>
		/// Return all entities containing all of the parameter type T with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get<T>(Comparison<Entity> comparer) where T : Component {
			return Get(comparer, new[] { typeof(T) });
		}

		/// <summary>
		/// Return all entities containing all of the component types
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get(params Type[] componentTypes) {
			var hashCode = FilteredCollection.GetHashCode(componentTypes);

			if (!_filteredCollections.ContainsKey(hashCode)) {
				_filteredCollections.Add(hashCode, new FilteredCollection(this, componentTypes));
			}

			return _filteredCollections[hashCode];
		}

		/// <summary>
		/// Return all entities containing all of the component types with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="componentTypes"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public FilteredCollection Get(IComparer<Entity> comparer, params Type[] componentTypes) {
			var hashCode = FilteredCollection.GetHashCode(componentTypes, comparer);

			if (!_filteredCollections.ContainsKey(hashCode)) {
				_filteredCollections.Add(hashCode, new FilteredCollection(this, componentTypes, comparer));
			}

			return _filteredCollections[hashCode];
		}

		/// <summary>
		/// Return all entities containing all of the component types with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="componentTypes"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public FilteredCollection Get(Comparison<Entity> comparer, params Type[] componentTypes) {
			return Get(new LambdaComparer<Entity>(comparer), componentTypes);
		}

		#endregion

		#region Create/Remove Entity

		/// <summary>
		/// Create and return a new entity
		/// </summary>
		/// <returns></returns>
		public Entity Create(IEnumerable<Component> comps = null) {
			var nextId = new UniqueId();

			// Ensure that the next available id is, in fact, available
			while (Entities.ContainsKey(nextId)) {
				nextId = new UniqueId();
			}

			var entity = comps == null ? new Entity(this, nextId) : new Entity(this, nextId, comps);
			
			FilteredCollections.Each(c => c.Add(entity));   // Add to filtered collections
			OnEntityAdded(entity);
			return entity;
		}		

		/// <summary>
		/// Remove an entity by id
		/// </summary>
		/// <param name="id"></param>
		public void Remove(UniqueId id) {
			Remove(Entities[id]);
		}

		/// <summary>
		/// Remove an entity
		/// </summary>
		/// <param name="entity"></param>
		public void Remove(Entity entity) {			
			FilteredCollections.Each(c => c.Remove(entity));    // Remove from filtered collections
			Components.Remove(entity);                       // Remove components
			Entities.Remove(entity.Id);                        // Remove from entity dictionary
			OnEntityRemoved(entity);
		}

		#endregion

		#region IEnumerable

		public IEnumerator<Entity> GetEnumerator() {
			return Entities.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Return all components belonging to the entity id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Component> All(UniqueId id) {
			Contract.Requires<ArgumentNullException>(id != null, "id");
			return Components.All(id);
		}
	}
}
