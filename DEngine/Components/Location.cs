using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using DEngine.Entity;

namespace DEngine.Components {
	public class Location : EntityComponent, IEquatable<Location> {
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

		public Location(Point position, Map level) {
			Position = position;
			Level = level;
		}

		public Location(int x, int y, Map level) : this(new Point(x, y), level) {}

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

		public bool Equals(Location other) {
			return !ReferenceEquals(null, other);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof (Location))
				return false;
			return Equals((Location) obj);
		}

		public override int GetHashCode() {
			return 0;
		}

		public static bool operator ==(Location left, Location right) {
			return Equals(left, right);
		}

		public static bool operator !=(Location left, Location right) {
			return !Equals(left, right);
		}
	}
}
