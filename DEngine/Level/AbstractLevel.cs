using System.Collections.Generic;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Level {
	public abstract class AbstractLevel : Map {
		
		protected AbstractLevel(Size size) : base(size) { }

		public abstract bool IsTransparent(int x, int y);
		public abstract bool IsWalkable(int x, int y);
		public abstract bool IsWalkable(Point v);
		public abstract bool IsTransparent(Point v);		

		public abstract IEnumerable<Entity> GetEntitiesAt(Point location);
		public abstract IEnumerable<Entity> GetEntitiesAt<T>(Point location) where T : Component;

		public abstract IEnumerable<Entity> GetEntitiesAt<T1, T2>(Point location)
				where T1 : Component
				where T2 : Component;

		public abstract IEnumerable<Entity> GetEntitiesAt<T1, T2, T3>(Point location)
				where T1 : Component
				where T2 : Component
				where T3 : Component;

		public abstract IEnumerable<Entity> GetEntities();
	}
}