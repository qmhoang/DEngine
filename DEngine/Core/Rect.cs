using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	/// <summary>
	/// Immuatable data type representing a rectangle.
	/// <remarks>that the terms Upper, Left, Right, Bottom and Top are meaningful only when
	/// Size.Width and Size.Height are both positive</remarks>
	/// </summary>
	[Serializable]
	public struct Rect {
		readonly Point topLeft;
		readonly Size size;

		#region Constructors
		public Rect(Point topLeft, Size size) {
			this.topLeft = topLeft;
			this.size = size;
		}

		public Rect(Point topLeft, Point bottomRight) {
			this.topLeft = topLeft;
			this.size = new Size(bottomRight.X - topLeft.X + 1,
				bottomRight.Y - topLeft.Y + 1);

		}

		public Rect(int x1, int y1, int x2, int y2)
			: this(new Point(x1, y1), new Point(x2, y2)) {
		}
		#endregion

		#region Properties
		public Size Size {
			get { return size; }
		}

		public Point TopLeft {
			get { return topLeft; }
		}

		public int Top {
			get { return topLeft.Y; }
		}

		public int Left {
			get { return topLeft.X; }
		}

		public int Bottom {
			get { return topLeft.Y + size.Height - 1; }
		}

		public int Right {
			get { return topLeft.X + size.Width - 1; }
		}

		public Point BottomRight {
			get {
				return new Point(Right, Bottom);
			}
		}

		public Point TopRight {
			get { return new Point(Right, Top); }
		}

		public Point BottomLeft {
			get { return new Point(Left, Bottom); }
		}

		public Point Center {
			get {
				int hCtr = (Left + Right) / 2;
				int vCtr = (Bottom + Top) / 2;

				return new Point(hCtr, vCtr);
			}
		}

		public Point TopCenter {
			get {
				return new Point(Center.X, Top);
			}
		}

		public Point RightCenter {
			get {
				return new Point(Right, Center.Y);
			}
		}

		public Point BottomCenter {
			get {
				return new Point(Center.X, Bottom);
			}
		}

		public Point LeftCenter {
			get {
				return new Point(Left, Center.Y);
			}
		}

		#endregion

		#region Public Methods

		public bool Contains(Point point) {
			if ((point.X >= topLeft.X) && (point.Y >= topLeft.Y) &&
				(point.X <= BottomRight.X) && (point.Y <= BottomRight.Y))
				return true;

			return false;
		}
		#endregion

		#region Static Methods
		public static Rect MoveBy(Rect rect, Size delta) {
			Point newTopLeft = new Point(rect.topLeft.X + delta.Width,
				rect.topLeft.Y + delta.Height);

			return new Rect(newTopLeft, rect.size);
		}

		public static Rect MoveTo(Rect rect, Point topLeft) {
			return new Rect(topLeft, rect.size);
		}

		/// <summary>
		/// Adds dx to left and right, and dy to top and bottom
		/// New width += dx*2, new height = dy*2
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public static Rect Inflate(Rect source, int dx, int dy) {
			Rect ret;
			Point newTopLeft;
			Size newSize;

			newTopLeft = new Point(source.topLeft.X - dx,
				source.topLeft.Y - dy);

			newSize = new Size(source.size.Width + dx * 2, source.size.Height + dy * 2);

			ret = new Rect(newTopLeft, newSize);

			return ret;
		}
		#endregion

		#region Overrides
		public override bool Equals(object obj) {
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			return Equals((Rect)obj);
		}

		public bool Equals(Rect rect) {
			return (this.topLeft == rect.topLeft && this.size == rect.size);
		}

		public static bool operator ==(Rect left, Rect right) {
			return left.Equals(right);
		}

		public static bool operator !=(Rect left, Rect right) {
			return !left.Equals(right);
		}

		public override int GetHashCode() {
			int hash = 7;
			hash = hash * 13 + topLeft.GetHashCode();
			hash = hash * 13 + size.GetHashCode();

			return hash;
		}

		public override string ToString() {
			return string.Format("{0} : {1}", topLeft, size);
		}
		#endregion
	}
}
