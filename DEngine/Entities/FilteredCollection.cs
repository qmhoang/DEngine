using System;
using System.Collections.Generic;
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
		readonly Type[] filter;
		readonly EntityManager manager;

		/// <summary>
		/// Sorted entities for iteration
		/// </summary>
		readonly SortedSet<Entity> entities;

		/// <summary>
		/// Filter hash code - computed against filters individually
		/// so that order doesn't matter.  Plus the comparer hashcode.
		/// </summary>
		readonly int hashCode;

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
				return entities.Count;
			}
		}


		#region Constructors

		/// <summary>
		/// Constructor with a null comparer
		/// </summary>
		/// <param name="entityManager"></param>
		/// <param name="types"></param>
		internal FilteredCollection(EntityManager entityManager, Type[] types) {
			entities = entities ?? new SortedSet<Entity>();
			hashCode = FilteredCollection.GetHashCode(types, null);

			// Check that all types are really components
			if (!types.All(t => typeof(Component).IsAssignableFrom(t))) {
				throw new Exception("Type is not of IComponent - cannot filter.");
			}

			manager = entityManager;
			filter = types;

			// Add any existing entities
			foreach (var entity in manager) {
				if (MatchesFilter(entity)) {
					Add(entity);
				}
			}
		}

		/// <summary>
		/// Constructor with lambda comparer
		/// </summary>
		/// <param name="entityManager"></param>
		/// <param name="types"></param>
		/// <param name="comparer"></param>
		internal FilteredCollection(EntityManager entityManager, Type[] types, Func<Entity, Entity, int> comparer)
			: this(entityManager, types, new LambdaComparer<Entity>(comparer)) {}

		/// <summary>
		/// Constructor with comparer
		/// </summary>
		/// <param name="entityManager"></param>
		/// <param name="types"></param>
		/// <param name="comparer"></param>
		internal FilteredCollection(EntityManager entityManager, Type[] types, IComparer<Entity> comparer)
				: this(entityManager, types) {
			entities = new SortedSet<Entity>(comparer);
			hashCode = FilteredCollection.GetHashCode(types, comparer);
		}

		#endregion

		/// <summary>
		/// Check if the entity belongs in this collection
		/// </summary>
		/// <param name="entity"></param>
		internal bool MatchesFilter(Entity entity) {
			return filter.All(entity.Has);
		}

		/// <summary>
		/// Check if the collection is filtered against a given type
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		internal bool ContainsType(Type t) {
			// We just use .Contains because collections shouldn't ever contain
			// that many elements so speed isn't a huge ordeal
			return filter.Contains(t);
		}

		#region Internal Add/Remove

		/// <summary>
		/// Add a new entity to the collection
		/// </summary>
		/// <param name="entity"></param>
		internal bool Add(Entity entity) {
			// Ensure that the entity matches the filter
			if (!MatchesFilter(entity))
				return false;

			entities.Add(entity);

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
			// Notify any listeners that an entity was removed
			if (OnEntityRemove != null && MatchesFilter(entity)) {				
				OnEntityRemove(entity);
			}

			entities.Remove(entity);
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
			return hashCode;
		}

		/// <summary>
		/// Get the hashcode for the type collection and the comparer.  The type collection can be in any order.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		public static int GetHashCode(Type[] types) {
			return GetHashCode(types, null);
		}

		/// <summary>
		/// Get the hashcode for the type collection and the comparer.  The type collection can be in any order.
		/// </summary>
		/// <param name="types"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static int GetHashCode(Type[] types, IComparer<Entity> comparer) {
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
			return entities.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion
	}
}