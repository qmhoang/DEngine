using System;
using System.Collections.Generic;
using System.Text;
using DEngine.Actor;
using DEngine.Extensions;

namespace DEngine.Entity {

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
		readonly Dictionary<UniqueId, Entity> entities;

		/// <summary>
		/// Filtered entity collections
		/// </summary>
		readonly Dictionary<int, FilteredCollection> filteredCollections;

		/// <summary>
		/// Template collections
		/// </summary>
		readonly TemplateCollections templateCollections;

		/// <summary>
		/// Store of all loaded components, indexed by type and entity id
		/// </summary>
		readonly IComponentManager components;

		/// <summary>
		/// Store of all loaded components, indexed by type and entity id
		/// </summary>
		internal IComponentManager Components {
			get {
				return components;
			}
		}

		/// <summary>
		/// Filtered Entity Collections
		/// </summary>
		internal IEnumerable<FilteredCollection> FilteredCollections {
			get {
				return filteredCollections.Values;
			}
		}

		/// <summary>
		/// Indexer by Entity Id
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Entity this[UniqueId index] {
			get {
				return entities[index];
			}
		}

		#region Constructors

		public EntityManager()
			: this(new ComponentManager()) { }

		public EntityManager(IComponentManager componentManager) {
			entities = new Dictionary<UniqueId, Entity>();
			filteredCollections = new Dictionary<int, FilteredCollection>();
			components = componentManager;
			templateCollections = new TemplateCollections(this);
		}

		#endregion

		#region Get FilteredCollections

		/// <summary>
		/// Return all entities containing all of the parameter type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get<T>() where T : EntityComponent {
			return Get(new[] { typeof(T) });
		}

		/// <summary>
		/// Return all entities containing all of the parameter type T with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get<T>(IComparer<Entity> comparer) where T : EntityComponent {
			return Get(comparer, new[] { typeof(T) });
		}

		/// <summary>
		/// Return all entities containing all of the parameter type T with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get<T>(Func<Entity, Entity, int> comparer) where T : EntityComponent {
			return Get(comparer, new[] { typeof(T) });
		}

		/// <summary>
		/// Return all entities containing all of the component types
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public FilteredCollection Get(params Type[] componentTypes) {
			var hashCode = FilteredCollection.GetHashCode(componentTypes);

			if (!filteredCollections.ContainsKey(hashCode)) {
				filteredCollections.Add(hashCode, new FilteredCollection(this, componentTypes));
			}

			return filteredCollections[hashCode];
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

			if (!filteredCollections.ContainsKey(hashCode)) {
				filteredCollections.Add(hashCode, new FilteredCollection(this, componentTypes, comparer));
			}

			return filteredCollections[hashCode];
		}

		/// <summary>
		/// Return all entities containing all of the component types with a comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="componentTypes"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public FilteredCollection Get(Func<Entity, Entity, int> comparer, params Type[] componentTypes) {
			return Get(new LambdaComparer<Entity>(comparer), componentTypes);
		}

		#endregion

		#region Get Template Collections

		/// <summary>
		/// Get a collection of entities loaded from a template type
		/// </summary>
		/// <typeparam name="TTemplate"></typeparam>
		/// <returns></returns>
		public TemplateCollection Templates<TTemplate>()
			where TTemplate : Template {
			return templateCollections.Get<TTemplate>();
		}

		#endregion

		#region Create/Remove Entity

		/// <summary>
		/// Create and return a new entity
		/// </summary>
		/// <returns></returns>
		public Entity Create(IEnumerable<EntityComponent> components = null) {
			var nextId = new UniqueId();

			// Ensure that the next available id is, in fact, available
			while (entities.ContainsKey(nextId)) {
				nextId = new UniqueId();
			}

			var entity = components == null ? new Entity(this, nextId) : new Entity(this, nextId, components);

			entities.Add(entity.Id, entity);               // Add to entity dictionary
			FilteredCollections.Each(c => c.Add(entity));   // Add to filtered collections
			templateCollections.Add(entity);               // Add to template collections

			return entity;
		}

		/// <summary>
		/// Remove an entity by id
		/// </summary>
		/// <param name="id"></param>
		public void Remove(UniqueId id) {
			Remove(entities[id]);
		}

		/// <summary>
		/// Remove an entity
		/// </summary>
		/// <param name="entity"></param>
		public void Remove(Entity entity) {			
			FilteredCollections.Each(c => c.Remove(entity));    // Remove from filtered collections
			templateCollections.Remove(entity);                // Remove from template collections
			Components.Remove(entity.Id);                       // Remove components
			entities.Remove(entity.Id);                        // Remove from entity dictionary
		}

		#endregion

		#region IEnumerable

		public IEnumerator<Entity> GetEnumerator() {
			return entities.Values.GetEnumerator();
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
		public IEnumerable<EntityComponent> All(UniqueId id) {
			return Components.All(id);
		}
	}
}
