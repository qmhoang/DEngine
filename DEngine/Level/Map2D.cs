using System;
using System.Diagnostics.Contracts;
using DEngine.Core;

namespace DEngine.Level {
	public class Map2D {
		private Rectangle _map;

		public Map2D(Size size) {
			this._map = new Rectangle(new Point(0, 0), size);
		}

		public int Width { get { return _map.Width; } }
		public int Height { get { return _map.Height; } }
		public Size Size { get { return _map.Size; } }

		[Pure]
		public bool IsInBounds(Point v) {
			return _map.Contains(v);
		}

		[Pure]
		public bool IsOnBorder(Point v) {
			return IsOnBorder(v.X, v.Y);
		}

		[Pure]
		public bool IsInBounds(int x, int y) {
			return _map.Contains(x, y);
		}

		[Pure]
		public bool IsOnBorder(int x, int y) {
			return x == 0 || y == 0 || x == Width - 1 || y == Height - 1;
		}
	}
}