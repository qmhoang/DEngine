using System;
using System.Collections.Generic;
using System.Linq;
using DEngine.Components;
using DEngine.Entities;

namespace DEngine.Core {	
	public abstract class Map {
		public Size Size { get; protected set; }
		internal FovMap FOVMap;
		public EntityManager EntityManager { get; protected set; }

		//todo entity LUT by location (use filteredcollection to get) if speed is needed
		
		protected Map(Size size) {
			Size = size;
			FOVMap = new FovMap(Size.Width, Size.Height);
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

		public bool IsVisible(Point p) {
			return IsVisible(p.X, p.Y);
		}

		public bool IsVisible(int x, int y) {
			if (!IsInBoundsOrBorder(x, y))
				throw new ArgumentOutOfRangeException();
			return FOVMap.IsVisible(x, y);
		}

		public bool IsWalkable(Point v) {
			return IsWalkable(v.X, v.Y);
		}
		
		public bool IsWalkable(int x, int y) {
			if (!IsInBoundsOrBorder(x, y))
				return false;
			return FOVMap.IsWalkable(x, y);
		}

		public bool IsTransparent(Point v) {
			return IsTransparent(v.X, v.Y);
		}

		public bool IsTransparent(int x, int y) {
			if (!IsInBoundsOrBorder(x, y))
				return false;
			return FOVMap.IsTransparent(x, y);
		}

		protected void SetVisibility(int x, int y, bool visible) {
			FOVMap.SetVisibility(x, y, visible);			
		}

		protected void SetTransparency(int x, int y, bool transparent) {
			FOVMap.SetTransparency(x, y, transparent);
		}

		protected void SetWalkable(int x, int y, bool walkable) {
			FOVMap.SetWalkable(x, y, walkable);
		}

		protected void SetProperties(int x, int y, bool transparent, bool walkable) {
			FOVMap.SetProperties(x, y, transparent, walkable);
		} 

		public void CalculateFOV(Point viewPoint, int viewableDistance) {
			ShadowCastingFOV.ComputeRecursiveShadowcasting(FOVMap, viewPoint.X, viewPoint.Y, viewableDistance, true);
		}

		public IEnumerable<Entity> GetEntitiesAt(Point location, params Type[] types) {
			var l = types.ToList();
			l.Add(typeof(Location));
			return EntityManager.Get(l.ToArray()).Where(e => e.Get<Location>().Position == location);
		}
	}
}