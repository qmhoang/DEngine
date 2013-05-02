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

		public int X { get; set; }
		public int Y { get; set; }

		public Point(int x, int y) : this() {
			X = x;
			Y = y;
		}

		#region Overloaded Operators
		public static Point operator +(Point v1, Point v2) {
			return new Point(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Point operator -(Point v1, Point v2) {
			return new Point(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static Point operator *(Point v, double scalar) {
			return new Point((int) Math.Round(v.X * scalar), (int) Math.Round(v.Y * scalar));
		}

		public static Point operator /(Point v, double scalar) {
			Contract.Requires<DivideByZeroException>(Math.Abs(scalar - 0) > Double.Epsilon);
			return new Point((int) Math.Round(v.X / scalar), (int) Math.Round(v.Y / scalar));
		}

		// unary negation
		public static Point operator -(Point v) {
			return new Point(-v.X, -v.Y);
		}
		#endregion

		#region Overloaded Equality
		public static bool operator ==(Point left, Point right) {
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right) {
			return !left.Equals(right);
		}
		#endregion

		public double Length {
			get { return Math.Sqrt(LengthSquared); }
		}

		public double LengthSquared {
			get { return (X * X) + (Y * Y); }
		}

		public static double Distance(Point v1, Point v2) {
			return Math.Sqrt(((v2.X - v1.X) * (v2.X - v1.X)) + ((v2.Y - v1.Y) * (v2.Y - v1.Y)));
		}

		public double DistanceTo(Point p) {
			return Distance(this, p);
		}

		public bool IsAdjacentTo(Point other) {
			// not adjacent to the exact same position
			if (this == other) return false;

			Point offset = this - other;

			return (Math.Abs(offset.X) <= 1) && (Math.Abs(offset.Y) <= 1);
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
		/// Returns a new Point whose coordinates are the coordinates of this Point
		/// with the given value added to the X coordinate. This method does not modify this Point instance.
		/// </summary>
		/// <param name="offset">Distance to offset the X coordinate.</param>
		/// <returns>A new Point offset by the given X coordinate.</returns>
		public Point ShiftX(int offset) {
			return new Point(X + offset, Y);
		}

		/// <summary>
		/// Returns a new Point whose coordinates are the coordinates of this Point
		/// with the given value added to the Y coordinate. This method does not modify this Point instance.
		/// </summary>
		/// <param name="offset">Distance to offset the Y coordinate.</param>
		/// <returns>A new Point offset by the given Y coordinate.</returns>
		public Point ShiftY(int offset) {
			return new Point(X, Y + offset);
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
			return String.Format("(X={0}, Y={1})", X, Y);
		}
	}
}