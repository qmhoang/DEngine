using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DEngine.Entities {
	public abstract class EventSubsystem {
		protected readonly FilteredCollection Collection;
		
		public Entity GetEntity(Component c) {
			Contract.Requires<ArgumentNullException>(c != null, "c");			
			return c.Entity;
		}


		protected EventSubsystem(EntityManager entityManager, params Type[] types) {
			Contract.Requires<ArgumentNullException>(entityManager != null, "entityManager");
			Contract.Requires<ArgumentNullException>(types != null, "types");
			
			Collection = entityManager.Get(types);

			Collection.OnEntityAdd += EntityAddedToCollection;
			Collection.OnEntityRemove += EntityRemovedFromCollection;
		}

		protected abstract void EntityAddedToCollection(Entity entity);
		protected abstract void EntityRemovedFromCollection(Entity entity);
	}
}
