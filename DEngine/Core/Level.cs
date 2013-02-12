using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using DEngine.Components;
using DEngine.Entities;

namespace DEngine.Core {
	public class Map {
		private Rect map;

		public Map(Size size) {
			this.map = new Rect(new Point(0, 0), size);
		}

		public int Width { get { return map.Width; } }
		public int Height { get { return map.Height; } }
		public Size Size { get { return map.Size; } }

		[Pure]
		public bool IsInBounds(Point v) {
			return map.Contains(v);
		}

		[Pure]
		public bool IsOnBorder(Point v) {
			return IsOnBorder(v.X, v.Y);
		}

		[Pure]
		public bool IsInBounds(int x, int y) {
			return map.Contains(x, y);
		}

		[Pure]
		public bool IsOnBorder(int x, int y) {
			return x == 0 || y == 0 || x == Width - 1 && y == Height - 1;
		}
	}

	public abstract class Level : Map {
		
		protected Level(Size size) : base(size) { }

		public abstract bool IsTransparent(int x, int y);
		public abstract bool IsWalkable(int x, int y);
		public abstract bool IsWalkable(Point v);
		public abstract bool IsTransparent(Point v);

		public abstract IEnumerable<Entity> GetEntitiesAt(Point location);
		public abstract IEnumerable<Entity> GetEntities();
	}
}