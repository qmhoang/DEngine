using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public struct Circle : IEquatable<Circle>, IEnumerable<Point> {
		private readonly Point _origin;
		private readonly int _radius;

		public Circle(int x, int y, int radius) : this(new Point(x, y), radius) { }

		public Circle(Point origin, int radius) {
			this._origin = origin;
			this._radius = radius;
		}

		#region Properties

		public Point Origin {
			get { return _origin; }
		}

		public int Radius {
			get { return _radius; }
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

		public double Area {
			get { return Radius * Radius * Math.PI; }
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Is current point within the circle
		/// </summary>
		/// <param name="centerOfCircle"></param>
		/// <returns></returns>
		public bool Contains(Point centerOfCircle) {
			return Contains(centerOfCircle.X, centerOfCircle.Y);
		}

		/// <summary>
		/// Is current point within the circle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains(int x, int y) {
			if (x < this.X - _radius || x > this.X + _radius)
				return false;
			if (y < this.Y - _radius || y > this.Y + _radius)
				return false;
			return (x - this.X) * (x - this.X) + (y - this.Y) * (y - this.Y) <= _radius * _radius;
		}
		#endregion

		#region Equals
		public bool Equals(Circle other) {
			return other._origin.Equals(_origin) && other._radius == _radius;
		}

		public IEnumerator<Point> GetEnumerator() {
			Rectangle bounds = new Rectangle(new Point(-Radius, -Radius), Point.One * (Radius + Radius + 1));

			foreach (Point pos in bounds) {
				if (pos.LengthSquared <= Radius * Radius) {
					yield return pos + Origin;
				}
			}
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
				return (_origin.GetHashCode() * 397) ^ _radius;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
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
