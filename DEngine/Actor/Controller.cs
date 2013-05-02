using System.Collections.Generic;
using DEngine.Actions;
using DEngine.Components;

namespace DEngine.Actor {
	/// <summary>
	/// This is the abstract class in which all controllers inherit from.  This is done because components cannot be inherited from.
	/// A controller basically "controls" an entity.  Emitting actions on which the entity executes.
	/// </summary>
	public abstract class Controller {
		public abstract IAction NextAction();

		public virtual bool HasActionsQueued {
			get { return false; }
		}

		public virtual void Enqueue(IAction action) { }
		public virtual void Disturb() { }
		public virtual void Cancel() { }

		public ControllerComponent Holder { get; protected internal set; }

		public abstract Controller Copy();
	}
}