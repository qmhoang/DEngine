using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public class FovMap {
		private struct Cell {
			public bool Transparent;
			public bool Walkable;
			public bool Visible;
		}

		private Cell[,] cells;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public FovMap(int width, int height) {
			Width = width;
			Height = height;
			cells = new Cell[Width,Height];
		}

		public void Clear() {
			cells = new Cell[Width,Height];
		}

		public void ClearVisibility() {
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					cells[i, j].Visible = false;
		}

		public bool IsTransparent(int x, int y) {
			return cells[x, y].Transparent;
		}

		public bool IsVisible(int x, int y) {
			return cells[x, y].Visible;
		}

		public bool IsWalkable(int x, int y) {
			return cells[x, y].Walkable;
		}

		internal void SetVisibility(int x, int y, bool visible) {
			cells[x, y].Visible = visible;
		}

		public void SetProperty(int x, int y, bool transparent, bool walkable) {
			cells[x, y].Transparent = transparent;
			cells[x, y].Walkable = walkable;
		}
	}
}