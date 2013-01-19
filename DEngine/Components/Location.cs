using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Components {
	public class Location : Component, IEquatable<Location> {
		private Point position;
		public Point Position {
			get { return position; }
			set {
				position = value;
				OnPositionChanged(new EventArgs<Point>(position));
			}
		}

		public Map Level { get; set; }

		public event ComponentEventHandler<EventArgs<Point>> PositionChanged;

		public void OnPositionChanged(EventArgs<Point> e) {
			ComponentEventHandler<EventArgs<Point>> handler = PositionChanged;
			if (handler != null)
				handler(this, e);
		}

		public event ComponentEventHandler<EventArgs<Map>> MapChanged;

		public void OnMapChanged(EventArgs<Map> e) {
			ComponentEventHandler<EventArgs<Map>> handler = MapChanged;
			if (handler != null)
				handler(this, e);
		}

		public int X {
			get { return Position.X; }			
		}

		public int Y {
			get { return Position.Y; }			
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
			return Position == other.Position && ReferenceEquals(Level, other.Level);
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

		public override Component Copy() {
			var location = new Location(X, Y, Level);
			if (PositionChanged != null)
				location.PositionChanged = (ComponentEventHandler<EventArgs<Point>>) PositionChanged.Clone();
			if (MapChanged != null)
				location.MapChanged = (ComponentEventHandler<EventArgs<Map>>) MapChanged.Clone();
			return location;
		}
	}
}
