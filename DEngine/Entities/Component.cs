using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DEngine.Actor;

namespace DEngine.Entities {
	/// <summary>
	/// Component abstract.  Components are just data containers so there's no
	/// standard functionality across all of them.  The only functionality they
	/// should contain are things like operator overloads, type conversions, or
	/// internal data manipulation (getters, setters).  A few will have events
	/// to indicate that a value has crossed a threshold (ie, event OnDied when 
	/// int Health == 0)
	/// 
	/// Owner can be overridden to allow component members to set internal data
	/// according to the owning entity
	/// </summary>
	public abstract class Component : ICopy<Component> {
		private UniqueId owner;

		[XmlIgnore]
		public virtual UniqueId OwnerUId {
			get {
				return owner;
			}
			internal set {
				// Ensure that the owner has not been set, and that it is being set to something valid
				if (owner == default(UniqueId) && value != default(UniqueId)) {
					owner = value;
				} else {
					throw new FieldAccessException("Cannot reset component to different entity.");
				}

				OnSetOwner();
			}
		}

		/// <summary>
		/// Called when the component owner is set.  Allows children to set additional data
		/// upon being assigned to an entity.
		/// </summary>
		protected virtual void OnSetOwner() { }

		/// <summary>
		/// Deep clone
		/// </summary>
		/// <returns></returns>
		public abstract Component Copy();
	}
}
