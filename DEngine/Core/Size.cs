using System;

namespace DEngine.Core {
	/// <summary>
	/// Immutable type representing anything that has a width and a height
	/// </summary>
	public struct Size : IEquatable<Size> {
		public int Width { get; set; }
		public int Height { get; set; }

		/// <summary>
		/// Returns true if width and height are equal to zero.
		/// </summary>
		public bool IsEmpty {
			get {
				return Width == 0 && Height == 0;
			}
		}
		
		public Size(int width, int height) : this() {
			Width = width;
			Height = height;
		}

		public Size(Size size) : this() {
			Width = size.Width;
			Height = size.Height;
		}

		public override bool Equals(object obj) {
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			return Equals((Size) obj);
		}

		public bool Equals(Size size) {
			return (this.Width == size.Width && this.Height == size.Height);
		}

		public override int GetHashCode() {
			int hash = 7;
			hash = hash * 13 + Width.GetHashCode();
			hash = hash * 13 + Height.GetHashCode();

			return hash;
		}

		/// <summary>
		/// Returns the area (width * height).
		/// </summary>
		public int Area { get { return Width * Height; } }

		public override string ToString() {
			return string.Format("(Width={0}, Height={1})", Width, Height);
		}

		public static bool operator ==(Size left, Size right) {
			return left.Equals(right);
		}

		public static bool operator !=(Size left, Size right) {
			return !(left == right);
		}

		public static Size operator +(Size s1, Size s2) {
			return new Size(s1.Width + s2.Width, s1.Height + s2.Height);
		}

		public static Size operator -(Size s1, Size s2) {
			return new Size(s1.Width - s2.Width, s1.Height - s2.Height);
		}

		public static Size operator *(Size s, double scalar) {
			return new Size((int) (s.Width * scalar), (int) (s.Height * scalar));
		}

		public static Size operator /(Size s, double scalar) {
			return new Size((int) (s.Width / scalar), (int) (s.Height / scalar));
		}
	}
}