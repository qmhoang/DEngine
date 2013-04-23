using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DEngine.Extensions;

namespace DEngine.Entities {
	/// <summary>
	/// Collection of entities filtered by common component types.  Much of the functionality
	/// is set internal as it should be hidden from actual game logic.  Public methods
	/// are just iteration stuff.  It's supposed to act like an IEnumerable collection
	/// externally.  New entities that match the collection filter are automatically added.
	/// </summary>
	public sealed class FilteredCollection : IEnumerable<Entity> {
		readonly Type[] _filter;
		readonly EntityManager _manager;

		/// <summary>
		/// Sorted entities for iteration
		/// </summary>
		readonly SortedSet<Entity> _entities;

		/// <summary>
		/// Filter hash code - computed against filters individually
		/// so that order doesn't matter.  Plus the comparer hashcode.
		/// </summary>
		readonly int _hashCode;

		/// <summary>
		/// Event for EntitySystems to run on adding an entity
		/// </summary>
		public event EntityEventHandler OnEntityAdd;

		/// <summary>
		/// Event for EntitySystems to run on removal of an entity
		/// </summary>
		public event EntityEventHandler OnEntityRemove;

		/// <summary>
		/// The number of entities in this collection
		/// </summary>
		public int Count {
			get {
				return _entities.Count;
			}
		}


		#region Constructors

		/// <summary>
		/// Constructor with a null comparer
		/// </summary>
		/// <param name="entityManager"></param>
		/// <param name="types"></param>
		/// <param name = "comparer"></param>
		internal FilteredCollection(EntityManager entityManager, Type[] types, IComparer<Entity> comparer = null) {
			_entities = comparer == null ? new SortedSet<Entity>() : new SortedSet<Entity>(comparer);
			_hashCode = FilteredCollection.GetHashCode(types, comparer);

			// Check that all types are really components
			if (!types.All(t => typeof(Component).IsAssignableFrom(t))) {
				throw new Exception("Type is not of IComponent - cannot filter.");
			}

			_manager = entityManager;
			_filter = types;

			// Add any existing entities
			_manager.Each(e => Add(e));			
		}

		/// <summary>
		/// Constructor with lambda comparer
		/// </summary>
		/// <param name="entityManager"></param>
		/// <param name="types"></param>
		/// <param name="comparer"></param>
		internal FilteredCollection(EntityManager entityManager, Type[] types, Comparison<Entity> comparer)
			: this(entityManager, types, new LambdaComparer<Entity>(comparer)) {}

		#endregion

		/// <summary>
		/// Check if the entity belongs in this collection
		/// </summary>
		/// <param name="entity"></param>
		internal bool MatchesFilter(Entity entity) {
			return _filter.All(entity.Has);
		}

		/// <summary>
		/// Check if the collection is filtered against a given type
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		internal bool ContainsType(Type t) {
			// We just use .Contains because collections shouldn't ever contain
			// that many elements so speed isn't a huge ordeal
			return _filter.Contains(t);
		}

		#region Internal Add/Remove

		/// <summary>
		/// Add a new entity to the collection
		/// </summary>
		/// <param name="entity"></param>
		internal bool Add(Entity entity) {
			Contract.Requires<ArgumentNullException>(entity != null, "entity");
			// Ensure that the entity matches the filter
			if (!MatchesFilter(entity))
				return false;

			_entities.Add(entity);

			// Notify any listeners that a new entity was added
			if (OnEntityAdd != null && MatchesFilter(entity)) {
				OnEntityAdd(entity);
			}

			return true;
		}

		/// <summary>
		/// Remove an entity from the collection
		/// </summary>
		/// <param name = "entity"></param>
		internal void Remove(Entity entity) {
			Contract.Requires<ArgumentNullException>(entity != null, "entity");
			// Notify any listeners that an entity was removed
			if (OnEntityRemove != null && MatchesFilter(entity)) {				
				OnEntityRemove(entity);
			}

			_entities.Remove(entity);
		}

		#endregion

		#region GetHashCode and helpers

		/// <summary>
		/// Get the hashcode for the collection - built from the filters
		/// so that order doesn't matter.  Comparerer hashcode is added
		/// in if it's been set
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return _hashCode;
		}

		/// <summary>
		/// Get the hashcode for the type collection and the comparer.  The type collection can be in any order.
		/// </summary>
		/// <param name="types"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static int GetHashCode(Type[] types, IComparer<Entity> comparer = null) {
			var hashCode = 0;

			// Hashcode is build off the filters, independent of their order
			for (int i = 0; i < types.Length; i++)
				hashCode += types[i].GetHashCode();

			// Add on comparer hashcode if set - comparers should overload their
			// own GetHashCode to return a unique constant.
			hashCode += (comparer == null) ? 0 : comparer.GetHashCode();

			return hashCode;
		}

		#endregion

		#region IEnumerable

		public IEnumerator<Entity> GetEnumerator() {
			return _entities.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion
	}
}