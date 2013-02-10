using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Entities;

namespace DEngine.Components {
	public class ActorComponent : Component {
		public ActorAction NextAction() {
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
		public Queue<ActorAction> Actions { get; private set; }

		public ActorComponent(AbstractActor actor, AP ap) {
			Actor = actor;
			AP = ap;
			Actor.Holder = this;
		}

		public override Component Copy() {
			throw new NotImplementedException();
		}
	}

	public abstract class AbstractActor {
		public virtual bool RequiresInput {
			get { return false; }
		}

		public abstract ActorAction NextAction();

		public virtual void Disturb() { }

		public virtual void Cancel() { }

		public ActorComponent Holder { get; protected internal set; }

		protected AbstractActor() { }		
	}
}
