using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using DEngine.Components;
using DEngine.Entities;

namespace DEngine.Core {
	public abstract class Map {
		public abstract int Width { get; }
		public abstract int Height { get; }

		[Pure]
		public bool IsInBounds(Point v) {
			return IsInBounds(v.X, v.Y);
		}

		[Pure]
		public bool IsInBoundsOrBorder(Point v) {
			return IsInBoundsOrBorder(v.X, v.Y);
		}

		[Pure]
		public bool IsOnBorder(Point v) {
			return IsOnBorder(v.X, v.Y);
		}

		[Pure]
		public bool IsInBounds(int x, int y) {
			return x >= 1 && y >= 1 && x < Width - 1 && y < Height - 1;
		}

		[Pure]
		public bool IsInBoundsOrBorder(int x, int y) {
			return x >= 0 && y >= 0 && x < Width && y < Height;
		}

		[Pure]
		public bool IsOnBorder(int x, int y) {
			return x == 0 || y == 0 || x == Width - 1 && y == Height - 1;
		}
	}

	public abstract class Level : Map {
		public Size Size { get; protected set; }

		protected Level(Size size) {
			Size = size;
		}

		public override int Width {
			get { return Size.Width; }
		}

		public override int Height {
			get { return Size.Height; }
		}

		public abstract bool IsTransparent(int x, int y);
		public abstract bool IsWalkable(int x, int y);
		public abstract bool IsWalkable(Point v);
		public abstract bool IsTransparent(Point v);

		public abstract IEnumerable<Entity> GetEntitiesAt(Point location);
		public abstract IEnumerable<Entity> GetEntities();
	}
}