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
	public interface IAction {
		int APCost { get; }
		ActionResult OnProcess();		
	}

	public abstract class ActorAction : IAction {
		public Entity Entity { get; private set; }
		public abstract int APCost { get; }
	
		protected ActorAction(Entity entity) {
			Contract.Requires<ArgumentNullException>(entity != null, "entity");			
			Contract.Requires<ArgumentException>(entity.Has<Components.ActorComponent>());
			Entity = entity;			
		}
		
		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(Entity != null);
		}

		public abstract ActionResult OnProcess();
	}
}
