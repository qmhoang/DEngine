using System.Diagnostics.Contracts;
using DEngine.Core;

namespace DEngine.Level {
	public class Map {
		private Rectangle map;

		public Map(Size size) {
			this.map = new Rectangle(new Point(0, 0), size);
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
			return x == 0 || y == 0 || x == Width - 1 || y == Height - 1;
		}
	}
}