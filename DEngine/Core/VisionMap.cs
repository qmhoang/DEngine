namespace DEngine.Core {
	public sealed class VisionMap : Map {
		private bool[,] cells;

		public override int Width { get; protected set; }
		public override int Height { get; protected set; }

		public VisionMap(Size size) : this(size.Width, size.Height) { }

		public VisionMap(int width, int height) {
			Width = width;
			Height = height;
			cells = new bool[width, height];
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
}