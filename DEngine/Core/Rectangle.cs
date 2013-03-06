﻿using System;
using System.Collections;
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
	public struct Rectangle : IEquatable<Rectangle>, IEnumerable<Point> {
		private readonly Point topLeft;
		private readonly Size size;

		#region Constructors

		public Rectangle(Point topLeft, Size size) {
			this.topLeft = topLeft;
			this.size = size;
		}

		/// <summary>
		/// Creates a Rectangle using the specificed location for the top-left and bottom-right (INCLUSIVE).
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="bottomRight"></param>
		public Rectangle(Point topLeft, Point bottomRight) {
			this.topLeft = topLeft;
			this.size = new Size(bottomRight.X - topLeft.X,
			                     bottomRight.Y - topLeft.Y);
		}

		public Rectangle(int x, int y, int width, int height) : this(new Point(x, y), new Size(width, height)) { }

		#endregion

		#region Properties

		public IEnumerable<Point> Points {
			get {
				for (int i = 0; i < Width; i++) {
					for (int j = 0; j < Height; j++) {
						yield return new Point(Left + i, Top + j);
					}
				}
			}
		}

		public int Width {
			get { return Size.Width; }			
		}

		public int Height {
			get { return Size.Height; }
		}

		public Size Size {
			get { return size; }
		}

		public Point TopLeft {
			get { return topLeft; }
		}

		/// <summary>
		/// Gets the x-coordinate of the top-left of the rectangle
		/// </summary>
		public int X {
			get { return topLeft.X; }
		}

		/// <summary>
		/// Gets the y-coordinate of the top-left edge of the rectangle
		/// </summary>
		public int Y {
			get { return topLeft.Y; }
		}

		/// <summary>
		/// Gets the y-coordinate of the top edge of the rectangle
		/// </summary>
		public int Top {
			get { return topLeft.Y; }
		}

		/// <summary>
		/// Gets the x-coordinate of the left edge of the rectangle
		/// </summary>
		public int Left {
			get { return topLeft.X; }
		}

		/// <summary>
		/// Gets the y-coordinate of the bottom edge (top + height) of the rectangle
		/// </summary>
		public int Bottom {
			get { return topLeft.Y + size.Height; }
		}

		/// <summary>
		/// Gets the x-coordinate of the right edge (left + width) of the rectangle
		/// </summary>
		public int Right {
			get { return topLeft.X + size.Width; }
		}

		public Point BottomRight {
			get { return new Point(Right, Bottom); }
		}

		public Point TopRight {
			get { return new Point(Right, Top); }
		}

		public Point BottomLeft {
			get { return new Point(Left, Bottom); }
		}

		public Point Center {
			get {
				return new Point((Left + Right) / 2, (Bottom + Top) / 2);
			}
		}

		public Point TopCenter {
			get { return new Point(Center.X, Top); }
		}

		public Point RightCenter {
			get { return new Point(Right, Center.Y); }
		}

		public Point BottomCenter {
			get { return new Point(Center.X, Bottom); }
		}

		public Point LeftCenter {
			get { return new Point(Left, Center.Y); }
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Does the rectangle contain the point.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Contains(Point point) {
			return Contains(point.X, point.Y);
		}

		/// <summary>
		/// Does the rectangle contain the position at X and Y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains(int x, int y) {
			return (x >= Left) && (y >= Top) &&
			       (x < Right) && (y < Bottom);
		}

		/// <summary>
		/// Determines if this rectangle completely contains the specified rectangle
		/// </summary>
		/// <param name = "rectangle"></param>
		/// <returns></returns>
		public bool Contains(Rectangle rectangle) {
			return Top <= rectangle.Top && Bottom >= rectangle.Bottom &&
				   Left <= rectangle.Left && Right >= rectangle.Right;
		}

		/// <summary>
		/// Determines if this rectangle intersects the specified rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public bool Intersects(Rectangle rectangle) {
			if (rectangle.Width <= 0 || rectangle.Height <= 0 || Width <= 0 || Height <= 0) {
				return false;
			}
			
			return ((rectangle.Right < rectangle.Left || rectangle.Right > Left) &&
			        (rectangle.Bottom < rectangle.Top || rectangle.Bottom > Top) &&
			        (Right < Left || Right > rectangle.Left) &&
			        (Bottom < Top || Bottom > rectangle.Top));
		}

		/// <summary>
		/// Returns a Rectangle that added the delta to the top-left position.  Width and Height remain the same.
		/// </summary>
		/// <param name="delta"></param>
		/// <returns>The new Rectangle</returns>
		public Rectangle MoveBy(Size delta) {
			Point newTopLeft = new Point(topLeft.X + delta.Width,
			                             topLeft.Y + delta.Height);

			return new Rectangle(newTopLeft, size);
		}

		/// <summary>
		/// Returns a new Rectangle with topLeft position.  Width and Height remain the same.
		/// </summary>
		/// <param name="newTopLeft"></param>
		/// <returns></returns>
		public Rectangle MoveTo(Point newTopLeft) {
			return new Rectangle(newTopLeft, size);
		}

		/// <summary>
		/// Returns a new rectangled that adds dx to left and right, and dy to top and bottom
		/// New width += dx*2, new height = dy*2
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public Rectangle Inflate(int dx, int dy) {
			return new Rectangle(topLeft.X - dx,
			                     topLeft.Y - dy, 
								 size.Width + dx * 2, 
								 size.Height + dy * 2);
		}

		#endregion

		#region Static Methods
		/// <summary>
		/// Returns a Rectangle that added the delta to the top-left position.  Width and Height remain the same.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="delta"></param>
		/// <returns></returns>
		public static Rectangle MoveBy(Rectangle rect, Size delta) {
			return rect.MoveBy(delta);
		}

		/// <summary>
		/// Returns a new Rectangle with topLeft position.  Width and Height remain the same.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="topLeft"></param>
		/// <returns></returns>
		public static Rectangle MoveTo(Rectangle rect, Point topLeft) {
			return rect.MoveTo(topLeft);
		}

		/// <summary>
		/// Adds dx to left and right, and dy to top and bottom
		/// New width += dx*2, new height = dy*2
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public static Rectangle Inflate(Rectangle source, int dx, int dy) {
			return source.Inflate(dx, dy);
		}

		#endregion
		
		#region Overrides

		public override bool Equals(object obj) {
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			return Equals((Rectangle) obj);
		}

		public bool Equals(Rectangle rect) {
			return (this.topLeft == rect.topLeft && this.size == rect.size);
		}

		public static bool operator ==(Rectangle left, Rectangle right) {
			return left.Equals(right);
		}

		public static bool operator !=(Rectangle left, Rectangle right) {
			return !left.Equals(right);
		}

		public override int GetHashCode() {
			int hash = 7;
			hash = hash * 13 + topLeft.GetHashCode();
			hash = hash * 13 + size.GetHashCode();

			return hash;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<Point> GetEnumerator() {
			return Points.GetEnumerator();
		}

		public override string ToString() {
			return string.Format("{0}, {1}", topLeft, size);
		}

		#endregion
	}
}