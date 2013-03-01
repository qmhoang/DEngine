using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DEngine.Actor;
using DEngine.Components;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Actions {
	[ContractClass(typeof(ActionContract))]
	public interface IAction {
		/// <summary>
		/// APCost should be greater than 0.  If you want a timeless action, return ActionResult.SuccessNoTime instead.
		/// </summary>
		int APCost { get; }
		ActionResult OnProcess();
	}

	[ContractClassFor(typeof(IAction))]
	public abstract class ActionContract : IAction {
		public int APCost {
			get {
				Contract.Ensures(Contract.Result<int>() > 0);
				return default(int);
			}
		}

		public abstract ActionResult OnProcess();
	}

	public abstract class ActorAction : IAction {
		public Entity Entity { get; private set; }
		public abstract int APCost { get; }
		public string EntityName { get { return Identifier.GetNameOrId(Entity); } }
	
		protected ActorAction(Entity entity) {
			Contract.Requires<ArgumentNullException>(entity != null, "entity");			
			Contract.Requires<ArgumentException>(entity.Has<ActorComponent>());
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
