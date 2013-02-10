using System;
using System.Diagnostics.Contracts;

namespace DEngine.Core {
	/// <summary>
	/// Immutable type representing 2d coordinates position in ints
	/// </summary>
	public struct Point : IEquatable<Point>, IEquatable<Direction> {
		public static Point Zero { get { return new Point(0, 0); } }
		public static Point Origin { get { return Zero; } }
		public static Point One { get { return new Point(1, 1); } }
		public static Point Invalid { get { return new Point(-1, -1); } }

		public int X { get; private set; }
		public int Y { get; private set; }

		public Point(Point v) : this(v.X, v.Y) {}

		public Point(int x, int y) : this() {
			X = x;
			Y = y;
		}

		public static Point operator +(Point v1, Point v2) {
			return new Point(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Point operator -(Point v1, Point v2) {
			return new Point(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static Point operator *(Point v, int scalar) {
			return new Point(v.X * scalar, v.Y * scalar);
		}

		public static Point operator /(Point v, int scalar) {
			Contract.Requires<DivideByZeroException>(scalar != 0);
			return new Point(v.X / scalar, v.Y / scalar);
		}

		public static bool operator ==(Point left, Point right) {
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right) {
			return !left.Equals(right);
		}
		
		public double Length {
			get { return DistanceTo(Zero); }
		}

		public static double Distance(Point v1, Point v2) {
			return Math.Sqrt(((v2.X - v1.X) * (v2.X - v1.X)) + ((v2.Y - v1.Y) * (v2.Y - v1.Y)));
		}

		public double DistanceTo(Point p) {
			return Distance(this, p);
		}

		/// <summary>
		/// Returns a new Point instance by adding dx to this.X and dy to this.Y.  This method
		/// does not modify this Point instance.
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public Point Shift(int dx, int dy) {
			return new Point(X + dx, Y + dy);
		}

		/// <summary>
		/// Is current point within the circle
		/// </summary>
		/// <param name="centerOfCircle"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public bool IsInCircle(Point centerOfCircle, double r) {
			return IsInCircle(centerOfCircle.X, centerOfCircle.Y, r);
		}

		public bool IsInCircle(int x, int y, double r) {
			if (this.X < x - r || this.X > x + r)
				return false;
			if (this.Y < y - r || this.Y > y + r)
				return false;
			return (this.X - x) * (this.X - x) + (this.Y - y) * (this.Y - y) <= r * r;
		}

		// override object.Equals
		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj))
				return false;
			if (obj.GetType() != typeof(Point))
				return false;
			return Equals((Point) obj);
		}

		// override object.GetHashCode
		public override int GetHashCode() {
			unchecked {
				return (X * 397) ^ Y;
			}
		}

		public bool Equals(Point other) {
			return other.X == X && other.Y == Y;
		}

		public bool Equals(Direction other) {
			return Equals(other.Offset);
		}

		public override string ToString() {
			return String.Format("({0}, {1})", X, Y);
		}
	}
}