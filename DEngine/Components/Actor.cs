using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Entities;

namespace DEngine.Components {
	public class ActorComponent : Component {
		public IAction NextAction() {
			return Actor.NextAction();
		}

		public void Disturb() {
			Actor.Disturb();
		}

		public void Cancel() {
			Actor.Cancel();
		}

		public AbstractActor Actor { get; private set; }
		public AP AP { get; private set; }

		public ActorComponent(AbstractActor actor, AP ap) {
			Actor = actor;
			AP = ap;
			Actor.Holder = this;
		}

		public void Enqueue(IAction action) {
			Actor.Enqueue(action);
		}

		public override Component Copy() {
			return new ActorComponent(Actor.Copy(), new AP(AP.ActionPointPerTurn, AP.ActionPoints));
		}
	}

	public abstract class AbstractActor {
		public virtual bool RequiresInput {
			get { return false; }
		}

		public virtual void Enqueue(IAction action) {
			Actions.Enqueue(action);
		}

		public abstract IAction NextAction();

		public virtual void Disturb() { }

		public virtual void Cancel() { }

		public ActorComponent Holder { get; protected internal set; }
		public Queue<IAction> Actions { get; private set; }

		protected AbstractActor() {
			Actions = new Queue<IAction>();
		}

		protected AbstractActor(Queue<IAction> actions) {
			Actions = actions;
		}

		public abstract AbstractActor Copy();
	}
}
