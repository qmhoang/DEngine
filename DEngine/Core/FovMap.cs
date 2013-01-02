using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	internal class FovMap {
		internal struct Cell {
			public bool Transparent;
			public bool Walkable;
			public bool Visible;
		}

		internal Cell[,] Cells;

		internal int Width { get; private set; }
		internal int Height { get; private set; }

		internal FovMap(int width, int height) {
			Width = width;
			Height = height;
			Cells = new Cell[Width,Height];
		}

		internal void Clear() {
			Cells = new Cell[Width,Height];
		}

		internal void ClearVisibility() {
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					Cells[i, j].Visible = false;
		}

		internal bool IsTransparent(int x, int y) {
			return Cells[x, y].Transparent;
		}

		internal bool IsVisible(int x, int y) {
			return Cells[x, y].Visible;
		}

		internal bool IsWalkable(int x, int y) {
			return Cells[x, y].Walkable;
		}

		internal void SetVisibility(int x, int y, bool visible) {
			Cells[x, y].Visible = visible;
		}

		internal void SetProperties(int x, int y, bool transparent, bool walkable) {
			Cells[x, y].Transparent = transparent;
			Cells[x, y].Walkable = walkable;
		}
	}
}