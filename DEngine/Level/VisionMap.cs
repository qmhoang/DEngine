using DEngine.Core;

namespace DEngine.Level {
	public sealed class VisionMap : Map {
		private bool[,] cells;
	

		public VisionMap(Size size) : base(size) {
			cells = new bool[Width, Height];
		}

		public VisionMap(int width, int height) : base(new Size(width, height)) { }

		public void ClearVisibility() {
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					cells[i, j] = false;
		}

		public bool IsVisible(int x, int y) {
			return IsInBounds(x, y) && cells[x, y];
		}

		public bool IsVisible(Point p) {
			return IsVisible(p.X, p.Y);
		}

		public void SetVisibility(int x, int y, bool visible) {
			cells[x, y] = visible;
		}

		public VisionMap Copy() {
			return new VisionMap(Size) {cells = (bool[,]) cells.Clone()};
		}
	}
}