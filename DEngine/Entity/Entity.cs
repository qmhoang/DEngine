using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DEngine.Actor;
using DEngine.Extensions;

namespace DEngine.Entity {
	/// <summary>
	/// Basic entity class.  Consists of an id and functions to interact with the manager.
	///  - This provides an interface to interact with an individual entity
	/// </summary>
	public sealed class Entity : IEquatable<Entity>, IComparable<Entity> {
		readonly UniqueId id;
		readonly string template;
		readonly EntityManager manager;
		bool isActive = true;

		/// <summary>
		/// Sets and toggles the entity as active or not
		/// </summary>
		public bool IsActive {
			get {
				return isActive;
			}
			set {
				// Only fire events if value is changing
				if (isActive != value) {
					isActive = value;

					if (value && OnEntityActivate != null) {
						OnEntityActivate(this);
					} else if (!value && OnEntityDeactivate != null) {
						OnEntityDeactivate(this);
					}
				}

				isActive = value;
			}
		}

		/// <summary>
		/// Entity Id
		/// </summary>
		public UniqueId Id {
			get {
				return id;
			}
		}

		/// <summary>
		/// The type of template this entity was loaded from, if any
		/// </summary>
		public string Template {
			get {
				return template;
			}
		}

		/// <summary>
		/// Event for for activating this entity
		/// </summary>
		public event EntityEventHandler OnEntityActivate;

		/// <summary>
		/// Event for deactivating this entity
		/// </summary>
		public event EntityEventHandler OnEntityDeactivate;


		/// <summary>
		/// Construct empty entity
		/// </summary>
		/// <param name = "manager"></param>
		/// <param name="id"></param>
		public Entity(EntityManager manager, UniqueId id) {
			if (manager == null) {
				throw new ArgumentNullException("manager");
			}

			this.id = id;
			this.manager = manager;
		}

		/// <summary>
		/// Construct with components
		/// </summary>
		/// <param name = "manager"></param>
		/// <param name="id"></param>
		/// <param name="components"></param>
		public Entity(EntityManager manager, UniqueId id, IEnumerable<Component> components)
			: this(manager, id) {
			this.manager.Components.Add(Id, components);
		}

		#region Add/Remove Components

		/// <summary>
		/// Add a new component to the entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="component"></param>
		/// <returns></returns>
		public Entity Add<T>(T component) where T : Component {
			manager.Components.Add(Id, component);

			// Add any updated entity to any filtered collections
			manager.FilteredCollections.Each(c => c.Add(this));
			return this;
		}

		public Entity Add(IEnumerable<Component> components) {
			manager.Components.Add(Id, components);

			manager.FilteredCollections.Each(c => c.Add(this));
			return this;
		}

		/// <summary>
		/// Remove a component from the entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Entity Remove<T>() where T : Component {
			// Remove Order - Filtered Collections, ComponentManager

			// Remove this from any filtered collections that it no longer matches
			manager.FilteredCollections.Each(c =>
			{
				if (c.ContainsType(typeof(T))) {
					c.Remove(this);
				}
			});

			// Remove from the component manager
			manager.Components.Remove<T>(Id);
			return this;
		}

		#endregion

		/// <summary>
		/// Return this as a component
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure] 
		public T Get<T>() where T : Component {
			return manager.Components.Get<T>(Id);
		}

		/// <summary>
		/// Checks if the entity contains the given component type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[Pure]
		public bool Has<T>() where T : Component {
			return Has(typeof(T));
		}

		/// <summary>
		/// Checks if the entity contains the given component type
		///  - Marked as internal - the generic version puts the type control
		///  up front here rather than down in the ComponentManager
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		internal bool Has(Type t) {
			return manager.Components.Contains(Id, t);
		}

		/// <summary>
		/// Return all components belonging to this entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<Component> Components {
			get {
				return manager.All(Id);
			}
		}

		#region IEquatable and IComparable

		/// <summary>
		/// IEquatable
		/// </summary>		
		/// <param name = "other"></param>
		/// <returns></returns>
		public bool Equals(Entity other) {
			return other.Id == Id;
		}

		/// <summary>
		/// IComparable
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(Entity other) {
			return Id.CompareTo(other.Id);
		}

		#endregion

		public override string ToString() {
			return Id.ToString();
		}

		/// <summary>
		/// Deep clone
		/// </summary>
		/// <returns></returns>
		public Entity Copy() {
			var entity = new Entity(manager, new UniqueId());

			foreach (var component in Components) {
				entity.Add(component.Copy());
			}

			return entity;
		}
	}
}