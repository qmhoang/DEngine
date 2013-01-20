using System;
using System.Collections.Generic;
using System.Linq;
using DEngine.Components;
using DEngine.Entities;

namespace DEngine.Core {
	public class VisibilityMap {
		private bool[,] cells;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public VisibilityMap(Size size) : this(size.Width, size.Height) { }

		public VisibilityMap(int width, int height) {
			Width = width;
			Height = height;
			cells = new bool[Width, Height];
		}

		public void ClearVisibility() {
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					cells[i, j] = false;
		}

		public bool IsVisible(int x, int y) {
			return cells[x, y];
		}

		public bool IsVisible(Point p) {
			return IsVisible(p.X, p.Y);
		}

		public void SetVisibility(int x, int y, bool visible) {
			cells[x, y] = visible;
		}
	}
	public abstract class Map {
		public Size Size { get; protected set; }
//		internal FovMap FOVMap;
		public EntityManager EntityManager { get; protected set; }

		internal struct Cell {
			public bool Transparent;
			public bool Walkable;
		}

		internal Cell[,] Cells;

		//todo entity LUT by location (use filteredcollection to get) if speed is needed
		
		protected Map(Size size) {
			Size = size;
//			FOVMap = new FovMap(Size.Width, Size.Height);
			Cells = new Cell[size.Width, size.Height];
		}

		public int Width {
			get { return Size.Width; }
		}

		public int Height {
			get { return Size.Height; }
		}

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
			return x >= 1 && y >= 1 && x < Size.Width - 1 && y < Size.Height - 1;
		}

		public bool IsInBoundsOrBorder(int x, int y) {
			return x >= 0 && y >= 0 && x < Size.Width && y < Size.Height;
		}

		public bool IsOnBorder(int x, int y) {
			return x == 0 || y == 0 || x == Size.Width - 1 && y == Size.Height - 1;
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

//		public bool IsVisible(Point p) {
//			return IsVisible(p.X, p.Y);
//		}
//
//		public bool IsVisible(int x, int y) {
//			if (!IsInBoundsOrBorder(x, y))
//				throw new ArgumentOutOfRangeException();
//			return FOVMap.IsVisible(x, y);
//		}

		public bool IsWalkable(Point v) {
			return IsWalkable(v.X, v.Y);
		}

		public bool IsTransparent(Point v) {
			return IsTransparent(v.X, v.Y);
		}

//		public void CalculateFOV(Point viewPoint, int viewableDistance) {
//			ShadowCastingFOV.ComputeRecursiveShadowcasting(FOVMap, viewPoint.X, viewPoint.Y, viewableDistance, true);
//		}

		public IEnumerable<Entity> GetEntitiesAt(Point location, params Type[] types) {
			var l = types.ToList();
			l.Add(typeof(Location));
			return EntityManager.Get(l.ToArray()).Where(e => e.Get<Location>().Position == location);
		}
	}
}