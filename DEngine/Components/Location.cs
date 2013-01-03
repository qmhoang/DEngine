using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using DEngine.Entity;

namespace DEngine.Components {
	public class Location : EntityComponent {
		public Point Position { get; set; }
		public Map Level { get; set; }

		public int X {
			get { return Position.X; }
			set { Position = new Point(value, Y); }
		}

		public int Y {
			get { return Position.Y; }
			set { Position = new Point(X, value); }
		}

		public Location(Point coordinate) {
			Position = coordinate;
		}

		public Location(int x, int y) {
			Position = new Point(x, y);			
		}

		public Location() {}

		public double DistanceTo(Location loc) {
			return Position.DistanceTo(loc.Position);
		}

		public double DistanceTo(Point p) {
			return Position.DistanceTo(p);
		}

		public bool IsNear(int x, int y, int radius) {
			return IsNear(new Point(x, y), radius);
		}

		public bool IsNear(Point point, int radius) {
			return point.IsInCircle(Position, radius);
		}
	}
}
