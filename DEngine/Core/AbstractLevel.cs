using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DEngine.Components;
using DEngine.Entities;

namespace DEngine.Core {
	public abstract class AbstractLevel : Map {
		
		protected AbstractLevel(Size size) : base(size) { }

		public abstract bool IsTransparent(int x, int y);
		public abstract bool IsWalkable(int x, int y);
		public abstract bool IsWalkable(Point v);
		public abstract bool IsTransparent(Point v);		

		public abstract IEnumerable<Entity> GetEntitiesAt(Point location);
		public abstract IEnumerable<Entity> GetEntities();
	}
}