using System.Collections.Generic;
using DEngine.Core;

namespace DEngine.Level {
	public class Bresenham {
		public static IEnumerable<Point> GeneratePointsFromLine(Point origin, Point end) {
			Bresenham b = new Bresenham(origin.X, origin.Y, end.X, end.Y);

			int x = origin.X;
			int y = origin.Y;
			while (!b.Step(ref x, ref y))
				yield return new Point(x, y);
		}

		private Bresenham(int xFrom, int yFrom, int xTo, int yTo) {
			_origx = xFrom;
			_origy = yFrom;
			_destx = xTo;
			_desty = yTo;
			_deltax = xTo - xFrom;
			_deltay = yTo - yFrom;
			if (_deltax > 0)
				_stepx = 1;
			else if (_deltax < 0)
				_stepx = -1;
			else
				_stepx = 0;
			if (_deltay > 0)
				_stepy = 1;
			else if (_deltay < 0)
				_stepy = -1;
			else
				_stepy = 0;
			if (_stepx * _deltax > _stepy * _deltay) {
				_e = _stepx * _deltax;
				_deltax *= 2;
				_deltay *= 2;
			} else {
				_e = _stepy * _deltay;
				_deltax *= 2;
				_deltay *= 2;
			}
		}

		private bool Step(ref int xCur, ref int yCur) {
			if (_stepx * _deltax > _stepy * _deltay) {
				if (_origx == _destx)
					return true;
				_origx += _stepx;
				_e -= _stepy * _deltay;
				if (_e < 0) {
					_origy += _stepy;
					_e += _stepx * _deltax;
				}
			} else {
				if (_origy == _desty)
					return true;
				_origy += _stepy;
				_e -= _stepx * _deltax;
				if (_e < 0) {
					_origx += _stepx;
					_e += _stepy * _deltay;
				}
			}
			xCur = _origx;
			yCur = _origy;
			return false;
		}

		private int _stepx;
		private int _stepy;
		private int _e;
		private int _deltax;
		private int _deltay;
		private int _origx;
		private int _origy;
		private int _destx;
		private int _desty;
	}
}