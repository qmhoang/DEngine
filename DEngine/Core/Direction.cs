using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public struct Direction : IEquatable<Direction>, IEquatable<Point> {
		/// <summary>
		/// Gets the "none" direction.
		/// </summary>
		public static Direction None { get { return new Direction(Point.Zero); } }

		public static Direction N { get { return new Direction(new Point(0, -1)); } }
		public static Direction NE { get { return new Direction(new Point(1, -1)); } }
		public static Direction E { get { return new Direction(new Point(1, 0)); } }
		public static Direction SE { get { return new Direction(new Point(1, 1)); } }
		public static Direction S { get { return new Direction(new Point(0, 1)); } }
		public static Direction SW { get { return new Direction(new Point(-1, 1)); } }
		public static Direction W { get { return new Direction(new Point(-1, 0)); } }
		public static Direction NW { get { return new Direction(new Point(-1, -1)); } }


		public static Direction North { get { return N; } }
		public static Direction South { get { return S; } }
		public static Direction West { get { return W; } }
		public static Direction East { get { return E; } }
		public static Direction Northwest { get { return NW; } }
		public static Direction Northeast { get { return NE; } }
		public static Direction Southwest { get { return SW; } }
		public static Direction Southeast { get { return SE; } }

		public Point Offset { get; private set; }

		public static implicit operator Point(Direction direction) {
			return direction.Offset;
		}

		public static implicit operator Direction(Point point) {
			return Towards(point);
		}

		/// <summary>
		/// Adds the offset of the given Direction to the Point.
		/// </summary>
		/// <param name="v1">Point to add the Direction to.</param>
		/// <param name="d2">Direction to offset the vector.</param>
		/// <returns>A new Point.</returns>
		public static Point operator +(Point v1, Direction d2) {
			return v1 + d2.Offset;
		}

		/// <summary>
		/// Adds the offset of the given Direction to the Point.
		/// </summary>
		/// <param name="d1">Direction to offset the vector.</param>
		/// <param name="v2">Point to add the Direction to.</param>
		/// <returns>A new Point.</returns>
		public static Point operator +(Direction d1, Point v2) {
			return d1.Offset + v2;
		}

		/// <summary>
		/// Enumerates the directions in clockwise order, starting with <see cref="N"/>.
		/// </summary>
		public static IList<Direction> ClockwiseList {
			get { return new List<Direction> { N, NE, E, SE, S, SW, W, NW }; }
		}

		/// <summary>
		/// Enumerates the directions in counterclockwise order, starting with <see cref="N"/>.
		/// </summary>
		public static IList<Direction> CounterclockwiseList {
			get { return new List<Direction> { N, NW, W, SW, S, SE, E, NE }; }
		}

		/// <summary>
		/// Enumerates the four main compass directions.
		/// </summary>
		public static IList<Direction> NsewList {
			get { return new List<Direction> { N, S, E, W }; }
		}

		/// <summary>
		/// Gets a Direction heading from the origin towards the given position.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static Direction Towards(Point pos) {
			int x = 0, y = 0;

			if (pos.X < 0) x = -1;
			if (pos.X > 0) x = 1;
			if (pos.Y < 0) y = -1;
			if (pos.Y > 0) y = 1;

			return new Direction(new Point(x, y));
		}

		public static bool operator ==(Direction left, Direction right) {
			return left.Equals(right);
		}

		public static bool operator !=(Direction left, Direction right) {
			return !left.Equals(right);
		}

		public static bool operator ==(Direction left, Point right) {
			return left.Equals(right);
		}

		public static bool operator !=(Direction left, Point right) {
			return !left.Equals(right);
		}

		public static bool operator ==(Point left, Direction right) {
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Direction right) {
			return !left.Equals(right);
		}

		/// <summary>
		/// Gets the <see cref="Direction"/> following this one in clockwise order.
		/// Will wrap around. If this direction is None, then returns None.
		/// </summary>
		public Direction Clockwise {
			get {
				if (this == N) return NE;
				if (this == NE) return E;
				if (this == E) return SE;
				if (this == SE) return S;
				if (this == S) return SW;
				if (this == SW) return W;
				if (this == W) return NW;
				if (this == NW) return N;
				else return None;
			}
		}

		/// <summary>
		/// Gets the <see cref="Direction"/> following this one in counterclockwise
		/// order. Will wrap around. If this direction is None, then returns None.
		/// </summary>
		public Direction Counterclockwise {
			get {
				if (this == N) return NW;
				if (this == NE) return N;
				if (this == E) return NE;
				if (this == SE) return E;
				if (this == S) return SE;
				if (this == SW) return S;
				if (this == W) return SW;
				if (this == NW) return W;
				else return None;
			}
		}

		public Direction RotateLeft90 { get { return Counterclockwise.Counterclockwise; } }

		public Direction RotateRight90 { get { return Clockwise.Clockwise; } }

		public Direction Rotate180 { get { return new Direction(Offset * -1); } }

		public override bool Equals(object obj) {
			// check the type
			if (obj == null) return false;
			if (!(obj is Direction)) return false;

			// call the typed Equals
			return Equals((Direction)obj);
		}

		public override int GetHashCode() {
			return Offset.GetHashCode();
		}

		public bool Equals(Direction other) {
			return Offset.Equals(other.Offset);
		}

		public bool Equals(Point other) {
			return Offset.Equals(other);
		}

		public override string ToString() {
			if (this == N) return "N";
			if (this == NE) return "NE";
			if (this == E) return "E";
			if (this == SE) return "SE";
			if (this == S) return "S";
			if (this == SW) return "SW";
			if (this == W) return "W";
			if (this == NW) return "NW";
			if (this == None) return "None";

			return Offset.ToString();
		}

		private Direction(Point direction) : this() {
			Offset = direction;
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(Offset.X >= -1 && Offset.X <= 1 &&
			                   Offset.Y >= -1 && Offset.Y <= 1);
		}
	}
}
