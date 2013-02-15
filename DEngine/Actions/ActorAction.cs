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
		protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public Entity Entity { get; private set; }
		public abstract int APCost { get; }

		public virtual bool RequiresPrompt { get { return false; } }	
		
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
