using System;
using System.Collections.Generic;
using DEngine.Actor;

namespace DEngine.Entity {
	/// <summary>
	/// A collection of all entities loaded as a particular template type
	/// </summary>
	public class TemplateCollection : IEnumerable<Entity> {
		readonly Type type;                // The template type of this collection
		readonly EntityManager manager;    // Entity manager
		readonly HashSet<UniqueId> entities;  // Entites matching this template type

		/// <summary>
		/// Event for EntitySystems to run on adding an entity
		/// </summary>
		public event EntityEventHandler OnEntityAdd;

		/// <summary>
		/// Event for EntitySystems to run on removal of an entity
		/// </summary>
		public event EntityEventHandler OnEntityRemove;

		public int Count {
			get {
				return entities.Count;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="entityManager"></param>
		/// <param name="type"></param>
		internal TemplateCollection(EntityManager entityManager, Type type) {
			manager = entityManager;
			this.type = type;

			entities = new HashSet<UniqueId>();
		}

		#region Internal Add/Remove Entity

		internal void Add(Entity e) {
			entities.Add(e.Id);

			if (OnEntityAdd != null) {
				OnEntityAdd(e);
			}
		}

		internal void Remove(Entity e) {
			entities.Remove(e.Id);

			if (OnEntityRemove != null) {
				OnEntityRemove(e);
			}
		}

		#endregion

		#region IEnumerable

		public IEnumerator<Entity> GetEnumerator() {
			foreach (var id in entities) {
				yield return manager[id];
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion
	}
}