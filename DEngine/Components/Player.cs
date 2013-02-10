using System.Collections.Generic;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Entities;

namespace DEngine.Components {
	public class Player : AbstractActor {
		public Player() : base() { }

		public override bool RequiresInput {
			get { return true; }
		}

		public override ActorAction NextAction() {
			return null;
		}

	}
}