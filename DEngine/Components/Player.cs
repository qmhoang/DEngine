using System.Collections.Generic;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Components {
	public class Player : AbstractActor {
		public Player() : base() { }

		protected Player(Queue<ActorAction> actions) : base(actions) {}

		public override bool RequiresInput {
			get { return base.Actions.Count == 0; }
		}

		public override ActorAction NextAction() {
			return Actions.Dequeue();
		}

		public override void Cancel() {
			base.Cancel();

			Actions.Dequeue();
		}

		public override AbstractActor Copy() {
			return new Player(new Queue<ActorAction>(Actions));					
		}

		public override void Disturb() {
			base.Disturb();

			Actions.Clear();
		}
	}
}