namespace DEngine.Core {
	public abstract class Map {
		public Size Size { get; protected set; }

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

		public abstract bool IsVisible(int x, int y);

		public bool IsWalkable(Point v) {
			return IsWalkable(v.X, v.Y);
		}

		public abstract bool IsWalkable(int x, int y);
	}
}