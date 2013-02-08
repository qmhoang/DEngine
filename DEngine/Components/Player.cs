using DEngine.Actions;
using DEngine.Actor;
using DEngine.Entities;

namespace DEngine.Components {
	public class Player : Actor {
		public Player() : base(new AP()) {
			this.action = null;
		}

		public Player(ActorAction action) : base(new AP()) {
			this.action = action;
		}

		public override bool RequiresInput {
			get { return action == null; }
		}

		public override Component Copy() {
			return new Player(action);
		}

		public override ActorAction NextAction() {
			ActorAction a = action;
			action = null;
			return a;
		}

		private ActorAction action;
	}
}