using System.Collections.Generic;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Components {
	public class Player : AbstractActor {
		public Player() { }

		protected Player(Queue<IAction> actions) : base(actions) { }

		public override bool RequiresInput {
			get { return Actions.Count == 0; }
		}

		public override IAction NextAction() {
			return Actions.Dequeue();
		}

		public override void Cancel() {
			base.Cancel();

			Actions.Dequeue();
		}

		public override AbstractActor Copy() {
			return new Player(new Queue<IAction>(Actions));					
		}

		public override void Disturb() {
			base.Disturb();

			Actions.Clear();
		}
	}
}