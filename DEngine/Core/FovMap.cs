using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	internal class FovMap {
		internal struct Cell {
			public bool Transparent;
			public bool Walkable;
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

		internal bool IsTransparent(int x, int y) {
			return Cells[x, y].Transparent;
		}


		internal bool IsWalkable(int x, int y) {
			return Cells[x, y].Walkable;
		}

		internal void SetTransparency(int x, int y, bool transparent) {
			Cells[x, y].Transparent = transparent;
		}

		internal void SetWalkable(int x, int y, bool walkable) {
			Cells[x, y].Walkable = walkable;
		}

		internal void SetProperties(int x, int y, bool transparent, bool walkable) {
			Cells[x, y].Transparent = transparent;
			Cells[x, y].Walkable = walkable;
		}

	}
}