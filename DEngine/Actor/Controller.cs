using System.Collections.Generic;
using DEngine.Actions;
using DEngine.Components;

namespace DEngine.Actor {
	public abstract class Controller {
		public virtual void Enqueue(IAction action) {
			Actions.Enqueue(action);
		}

		public abstract IAction NextAction();

		public virtual void Disturb() { }

		public virtual void Cancel() { }

		public ActorComponent Holder { get; protected internal set; }
		public Queue<IAction> Actions { get; private set; }

		protected Controller() {
			Actions = new Queue<IAction>();
		}

		protected Controller(Queue<IAction> actions) {
			Actions = actions;
		}

		public abstract Controller Copy();
	}
}