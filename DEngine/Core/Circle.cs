using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public struct Circle : IEquatable<Circle> {
		private readonly Point origin;
		private readonly int radius;

		public Circle(int x, int y, int radius) : this(new Point(x, y), radius) { }

		public Circle(Point origin, int radius) {
			this.origin = origin;
			this.radius = radius;
		}

		#region Properties

		public Point Origin {
			get { return origin; }
		}

		public int Radius {
			get { return radius; }
		}

		/// <summary>
		/// Gets the x-coordinate of the origin
		/// </summary>
		public int X {
			get { return Origin.X; }
		}

		/// <summary>
		/// Gets the y-coordinate of the origin
		/// </summary>
		public int Y {
			get { return Origin.Y; }
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Is current point within the circle
		/// </summary>
		/// <param name="centerOfCircle"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public bool Contains(Point centerOfCircle) {
			return Contains(centerOfCircle.X, centerOfCircle.Y);
		}

		public bool Contains(int x, int y) {
			if (x < this.X - radius || x > this.X + radius)
				return false;
			if (y < this.Y - radius || y > this.Y + radius)
				return false;
			return (x - this.X) * (x - this.X) + (y - this.Y) * (y - this.Y) <= radius * radius;
		}
		#endregion

		#region Equals
		public bool Equals(Circle other) {
			return other.origin.Equals(origin) && other.radius == radius;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj))
				return false;
			if (obj.GetType() != typeof(Circle))
				return false;
			return Equals((Circle) obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (origin.GetHashCode() * 397) ^ radius;
			}
		}

		public static bool operator ==(Circle left, Circle right) {
			return left.Equals(right);
		}

		public static bool operator !=(Circle left, Circle right) {
			return !left.Equals(right);
		}
		#endregion
	}
}
