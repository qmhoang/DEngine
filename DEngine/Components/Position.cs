using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using DEngine.Entity;

namespace DEngine.Components {
	public class Position : EntityComponent {
		public Point Coordinates { get; set; }

		public int X {
			get { return Coordinates.X; }
			set { Coordinates = new Point(value, Y); }
		}

		public int Y {
			get { return Coordinates.Y; }
			set { Coordinates = new Point(X, value); }
		}

		public Position(Point coordinate) {
			Coordinates = coordinate;
		}

		public Position(int x, int y) {
			Coordinates = new Point(x, y);			
		}

		public double DistanceTo(Position p) {
			return Coordinates.DistanceTo(p.Coordinates);
		}

		public bool IsNear(int x, int y, int radius) {
			return IsNear(new Point(x, y), radius);
		}

		public bool IsNear(Point point, int radius) {
			return point.IsInCircle(Coordinates, radius);
		}
	}
}
