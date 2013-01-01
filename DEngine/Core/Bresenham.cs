using System.Collections.Generic;

namespace DEngine.Core {
	public class Bresenham {
		public static List<Point> GeneratePointsFromLine(Point origin, Point end) {
			var list = new List<Point>();
			Bresenham b = new Bresenham(origin.X, origin.Y, end.X, end.Y);

			int x = origin.X;
			int y = origin.Y;
			while (!b.Step(ref x, ref y))
				list.Add(new Point(x, y));

			return list;
		}

		private Bresenham(int xFrom, int yFrom, int xTo, int yTo) {
			origx = xFrom;
			origy = yFrom;
			destx = xTo;
			desty = yTo;
			deltax = xTo - xFrom;
			deltay = yTo - yFrom;
			if (deltax > 0)
				stepx = 1;
			else if (deltax < 0)
				stepx = -1;
			else
				stepx = 0;
			if (deltay > 0)
				stepy = 1;
			else if (deltay < 0)
				stepy = -1;
			else
				stepy = 0;
			if (stepx * deltax > stepy * deltay) {
				e = stepx * deltax;
				deltax *= 2;
				deltay *= 2;
			} else {
				e = stepy * deltay;
				deltax *= 2;
				deltay *= 2;
			}
		}

		private bool Step(ref int xCur, ref int yCur) {
			if (stepx * deltax > stepy * deltay) {
				if (origx == destx)
					return true;
				origx += stepx;
				e -= stepy * deltay;
				if (e < 0) {
					origy += stepy;
					e += stepx * deltax;
				}
			} else {
				if (origy == desty)
					return true;
				origy += stepy;
				e -= stepx * deltax;
				if (e < 0) {
					origx += stepx;
					e += stepy * deltay;
				}
			}
			xCur = origx;
			yCur = origy;
			return false;
		}

		private int stepx;
		private int stepy;
		private int e;
		private int deltax;
		private int deltay;
		private int origx;
		private int origy;
		private int destx;
		private int desty;
	}
}