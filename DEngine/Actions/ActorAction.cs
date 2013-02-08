using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Actions {
	public abstract class ActorAction {
		public Entity Entity { get; private set; }
		public int APCost { get; private set; }
		public Queue<ActorAction> Actions { get; private set; }

		protected ActorAction(Entity entity, int apcost) {
			Contract.Requires<ArgumentNullException>(entity != null, "entity");
			Contract.Requires<ArgumentException>(apcost >= 0);
			Contract.Requires<ArgumentException>(entity.Has<Components.Actor>());
			Entity = entity;
			APCost = apcost;
			Actions = new Queue<ActorAction>();
		}

		
		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(Entity != null);
			Contract.Invariant(Actions != null);
		}

		public abstract ActionResult OnProcess();

		private bool usesAP;
	}
}
