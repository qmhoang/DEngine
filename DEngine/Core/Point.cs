using System;
using System.Diagnostics.Contracts;

namespace DEngine.Core {
	/// <summary>
	/// Pair of ints (x, y) representing 2d coordinates position
	/// </summary>
	public struct Point {
		public static readonly Point Zero = new Point(0, 0);
		public static readonly Point Origin = new Point(0, 0);
		public static readonly Point One = new Point(1, 1);
		public static readonly Point North = new Point(0, -1);
		public static readonly Point South = new Point(0, 1);
		public static readonly Point West = new Point(-1, 0);
		public static readonly Point East = new Point(1, 0);
		public static readonly Point Northwest = North + West;
		public static readonly Point Northeast = North + East;
		public static readonly Point Southwest = South + West;
		public static readonly Point Southeast = South + East;
		public static readonly Point Invalid = new Point(-1, -1);

		public int X { get; private set; }
		public int Y { get; private set; }

		public Point(Point v) : this(v.X, v.Y) {}

		public Point(int x, int y) : this() {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Get direction left of heading, same magnitude
		/// </summary>
		/// <returns></returns>
		public Point Left {
			get { return new Point(Y, -X); }
		}

		/// <summary>
		/// Get direction right of heading, same magnitude
		/// </summary>
		/// <returns></returns>
		public Point Right {
			get { return new Point(-Y, X); }
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

		public static bool operator ==(Point v1, Point v2) {
			return v1.X == v2.X && v1.Y == v2.Y;
		}

		public static bool operator !=(Point v1, Point v2) {
			return !(v1 == v2);
		}

		public double Length() {
			return DistanceTo(Zero);
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

		//        public bool IsInRectangle(Position topLeft, Position bottomRight) {
		//            return X >= topLeft.X && X <= bottomRight.X && Y >= topLeft.Y && Y <= bottomRight.Y;
		//        }

		// override object.Equals
		public override bool Equals(object obj) {
			//       
			// See the full list of guidelines at
			//   http://go.microsoft.com/fwlink/?LinkID=85237  
			// and also the guidance for operator== at
			//   http://go.microsoft.com/fwlink/?LinkId=85238
			//

			if (obj == null || GetType() != obj.GetType())
				return false;

			return this == (Point) obj;
		}

		// override object.GetHashCode
		public override int GetHashCode() {
			return X ^ Y;
		}

		public override string ToString() {
			return String.Format("({0}, {1})", X, Y);
		}
	}
}