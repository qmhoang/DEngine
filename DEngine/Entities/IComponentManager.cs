using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DEngine.Actor;

namespace DEngine.Entities {
	/// <summary>
	/// Interface for a ComponentManager.  Handles storage and retrieval of component
	/// types associated with entity ids.
	/// </summary>
	[ContractClass(typeof(IComponentManagerContract))]
	public interface IComponentManager : IEnumerable<Type> {
		/// <summary>
		/// Add a new component to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <param name="o"></param>
		void Add<T>(Entity e, T o) where T : Component;

		/// <summary>
		/// Add a list of components to an entity
		/// </summary>
		/// <param name="e"></param>
		/// <param name="components"></param>
		void Add(Entity e, IEnumerable<Component> components);

		/// <summary>
		/// Remove all components belonging to an entity
		/// </summary>
		/// <param name="e"></param>
		void Remove(Entity e);

		/// <summary>
		/// Remove a component belonging to an entity
		/// </summary>
		/// <param name="e"></param>		
		/// <returns>If a component was removed or not</returns>
		bool Remove<T>(Entity e) where T : Component;

		/// <summary>
		/// Get a component beloning to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <returns></returns>
		T Get<T>(Entity e) where T : Component;

		/// <summary>
		/// Check if an entity has a component type
		/// </summary>
		/// <param name="e"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		bool Contains(Entity e, Type t);

		IEnumerable<Component> All(UniqueId id);
	}

// ReSharper disable InconsistentNaming
	[ContractClassFor(typeof(IComponentManager))]
	abstract class IComponentManagerContract : IComponentManager {
// ReSharper restore InconsistentNaming
		public void Add<T>(Entity e, T o) where T : Component {
			Contract.Requires<ArgumentNullException>(o != null, "o");
			Contract.Requires<ArgumentNullException>(e != null, "e");
		}

		public void Add(Entity e, IEnumerable<Component> components) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			Contract.Requires<ArgumentNullException>(components != null, "comps");	
		}

		public void Remove(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "id");			
		}

		public bool Remove<T>(Entity e) where T : Component {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			return false;
		}

		public T Get<T>(Entity e) where T : Component {
			Contract.Requires<ArgumentNullException>(e != null, "id");
			Contract.Ensures(Contract.Result<T>() != null);
			return default(T);
		}

		public bool Contains(Entity e, Type t) {
			Contract.Requires<ArgumentNullException>(e != null, "id");
			return false;
		}

		public IEnumerable<Component> All(UniqueId id) {
			Contract.Requires<ArgumentNullException>(id != null, "id");
			return default(IEnumerable<Component>);
		}

		public abstract IEnumerator<Type> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}