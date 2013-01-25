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

		public bool IsInBounds(Point v) {
			return IsInBounds(v.X, v.Y);
		}

		public bool IsInBoundsOrBorder(Point v) {
			return IsInBoundsOrBorder(v.X, v.Y);
		}

		public bool IsOnBorder(Point v) {
			return IsOnBorder(v.X, v.Y);
		}

		public bool IsInBounds(int x, int y) {
			return x >= 1 && y >= 1 && x < Width - 1 && y < Height - 1;
		}

		public bool IsInBoundsOrBorder(int x, int y) {
			return x >= 0 && y >= 0 && x < Width && y < Height;
		}

		public bool IsOnBorder(int x, int y) {
			return x == 0 || y == 0 || x == Width - 1 && y == Height - 1;
		}
	}

	public abstract class Level : Map {
		public Size Size { get; protected set; }
		public EntityManager EntityManager { get; protected set; }

		internal struct Cell {
			public bool Transparent;
			public bool Walkable;
		}

		internal Cell[,] Cells;

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(Cells != null);
			Contract.Invariant(EntityManager != null);
		}

		//todo entity LUT by location (use filteredcollection to get) if speed is needed
		
		protected Level(Size size) {
			Size = size;
			Cells = new Cell[size.Width, size.Height];
		}

		public override int Width {
			get { return Size.Width; }
		}

		public override int Height {
			get { return Size.Height; }
		}

		public void Clear() {
			Cells = new Cell[Width, Height];
		}

		public bool IsTransparent(int x, int y) {
			if (!IsInBoundsOrBorder(x, y))
				return false;
			return Cells[x, y].Transparent;
		}
		
		public bool IsWalkable(int x, int y) {
			if (!IsInBoundsOrBorder(x, y))
				return false;
			return Cells[x, y].Walkable;
		}

		public void SetTransparency(int x, int y, bool transparent) {
			Cells[x, y].Transparent = transparent;
		}

		public void SetWalkable(int x, int y, bool walkable) {
			Cells[x, y].Walkable = walkable;
		}

		public void SetProperties(int x, int y, bool transparent, bool walkable) {
			Cells[x, y].Transparent = transparent;
			Cells[x, y].Walkable = walkable;
		}

		public bool IsWalkable(Point v) {
			return IsWalkable(v.X, v.Y);
		}

		public bool IsTransparent(Point v) {
			return IsTransparent(v.X, v.Y);
		}

		public IEnumerable<Entity> GetEntitiesAt(Point location, params Type[] types) {
			Contract.Ensures(Contract.Result<IEnumerable<Entity>>().All(e => e != null && types.All(e.Has) && e.Has<Location>()));
			var l = types.ToList();
			l.Add(typeof(Location));			
			return EntityManager.Get(l.ToArray()).Where(e => e.Get<Location>().Position == location);
		}
	}
}