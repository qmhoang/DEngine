using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DEngine.Components.Actions;
using DEngine.Core;
using DEngine.Entities;
using DEngine.Extensions;

namespace DEngine.Components {
	public class PositionChangedEvent : EventArgs {
		public Point Previous { get; private set; }
		public Point Current { get; private set; }

		public PositionChangedEvent(Point prev, Point curr) {
			Previous = prev;
			Current = curr;
		}
	}

	public abstract class Position : Component, IEquatable<Position> {
		private Point point;
		public Point Point {
			get { return point; }
			set {
				var eventArgs = new PositionChangedEvent(point, value);
				OnPositionChanged(eventArgs);
				point = value;
			}
		}

		public event ComponentEventHandler<PositionChangedEvent> PositionChanged;

		public void OnPositionChanged(PositionChangedEvent e) {
			ComponentEventHandler<PositionChangedEvent> handler = PositionChanged;
			if (handler != null)
				handler(this, e);
		}

		public int X {
			get { return Point.X; }			
		}

		public int Y {
			get { return Point.Y; }			
		}

		protected Position(Point position) {
			Point = position;
		}

		public Position(int x, int y) : this(new Point(x, y)) {}

		public double DistanceTo(Position loc) {
			Contract.Requires<ArgumentNullException>(loc != null, "loc");
			return Point.DistanceTo(loc.Point);
		}

		public double DistanceTo(Point p) {
			return Point.DistanceTo(p);
		}

		public bool IsNear(int x, int y, int radius) {
			return IsNear(new Point(x, y), radius);
		}

		public bool IsNear(Point p, int radius) {
			return p.IsInCircle(Point, radius);
		}

		public bool Equals(Position other) {
			return Point == other.Point;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof (Position))
				return false;
			return Equals((Position) obj);
		}

		public override int GetHashCode() {
			return 0;
		}

		public static bool operator ==(Position left, Position right) {
			return Equals(left, right);
		}

		public static bool operator !=(Position left, Position right) {
			return !Equals(left, right);
		}		
	}
}
