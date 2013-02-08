using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Entities;

namespace DEngine.Components {
	public abstract class Actor : Component {
		public virtual bool RequiresInput {
			get { return false; }
		}

		public abstract ActorAction NextAction();

		public virtual void Disturb() { }

		public virtual void Cancel() { }

		public AP AP { get; private set; }

		protected Actor(AP ap) {
			AP = ap;
		}		
	}
}
