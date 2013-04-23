using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class RectangleTests {
		[Test]
		public void TestEmptyConstructor() {
			var r = new Rectangle();
			Assert.AreEqual(0, r.TopLeft.X);
			Assert.AreEqual(0, r.TopLeft.Y);
			Assert.AreEqual(0, r.Width);
			Assert.AreEqual(0, r.Height);

			Assert.AreEqual(0, r.Center.X);
			Assert.AreEqual(0, r.Center.Y);
		}

		[Test]
		public void TestFilledConstructor() {
			var r = new Rectangle(15, 25, 35, 45);
			Assert.AreEqual(15, r.TopLeft.X);
			Assert.AreEqual(25, r.TopLeft.Y);
			Assert.AreEqual(35, r.Width);
			Assert.AreEqual(45, r.Height);
		}

		[Test]
		public void TestConstructorEquality() {
			var r1 = new Rectangle(0, 0, 2, 2);
			var r2 = new Rectangle(new Point(0, 0), new Size(2, 2));
			var r3 = new Rectangle(new Point(0, 0), new Point(2, 2));

			Assert.AreEqual(r1, r2);
			Assert.AreEqual(r2, r3); // by transitive r1 = r3 also
		}

		[Test]
		public void TestEdges() {
			var r = new Rectangle(0, 0, 2, 2);
			Assert.AreEqual(0, r.Top);
			Assert.AreEqual(0, r.Left);
			Assert.AreEqual(2, r.Right);
			Assert.AreEqual(2, r.Bottom);
		}

		[Test]
		public void TestEdgesNegative() {
			var r = new Rectangle(-15, -15, 30, 30);
			Assert.AreEqual(-15, r.Top);
			Assert.AreEqual(-15, r.Left);
			Assert.AreEqual(15, r.Right);
			Assert.AreEqual(15, r.Bottom);
		}

		[Test]
		public void TestCenter() {
			var r = new Rectangle(-1, -1, 2, 2); // 2x2, centered on 0,0
			Assert.AreEqual(0, r.Center.X);
			Assert.AreEqual(0, r.Center.Y);
		}

		[Test]
		public void TestOffCenter() {
			var r = new Rectangle(0, 0, 2, 2);
			Assert.AreEqual(1, r.Center.X);
			Assert.AreEqual(1, r.Center.Y);

			r = new Rectangle(0, 0, 3, 3);
			Assert.AreEqual(1, r.Center.X);
			Assert.AreEqual(1, r.Center.Y);
		}

		[Test]
		public void TestContainsRect() {
			var outer = new Rectangle(0, 0, 10, 10);
			var inner = new Rectangle(5, 5, 2, 2);

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public void TestEmptyContainsRect() {
			var outer = new Rectangle(-1, -1, 2, 2);
			var inner = new Rectangle();

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public void TestContainsPoint(
				[Random(0, 99, 4)] int px,
				[Random(0, 99, 4)] int py) {
			var outer = new Rectangle(0, 0, 100, 100);
			var point = new Point(px, py);

			Assert.IsTrue(outer.Contains(point));
		}

		[Test]
		public void TestDoesNotContainRect() {
			var outer = new Rectangle(0, 1, 2, 2);
			var inner = new Rectangle(0, 0, 2, 2);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public void TestDoesNotContainPoint() {
			var outer = new Rectangle(0, 0, 1, 1);
			var inner = new Point(5, 5);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public void TestEmptyDoesNotContainPoint() {
			var outer = new Rectangle();
			var inner = new Point(5, 5);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public void TestEmptyDoesNotContainOrigin() {
			var outer = new Rectangle();
			var inner = new Point();

			Assert.IsFalse(outer.Contains(inner)); // matches java.awt.Rectangle and System.Drawing.Rectangle behavior
		}

		[Test]
		public void TestEmptyIntersectsFails() {
			var outer = new Rectangle();
			var inner = new Rectangle();

			Assert.IsFalse(outer.Intersects(inner)); // matches java.awt.Rectangle and System.Drawing.Rectangle behavior
		}

		[Test]
		public void TestIntersects() {
			var r1 = new Rectangle(0, 0, 20, 20);
			var r2 = new Rectangle(5, 5, 20, 20);

			Assert.IsTrue(r1.Intersects(r2));
			Assert.IsTrue(r2.Intersects(r1));


			r1 = new Rectangle(0, 0, 5, 5);
			r2 = new Rectangle(3, 3, 5, 5);
			Assert.IsTrue(r1.Intersects(r2));
			Assert.IsTrue(r2.Intersects(r1));
		}

		[Test]
		public void TestDoesNotIntersect() {
			var r1 = new Rectangle(0, 0, 2, 2);
			var r2 = new Rectangle(5, 5, 2, 2);

			Assert.IsFalse(r1.Intersects(r2));
			Assert.IsFalse(r2.Intersects(r1));

			r1 = new Rectangle(0, 0, 2, 2);
			r2 = new Rectangle(2, 2, 1, 1);

			Assert.IsFalse(r1.Intersects(r2));
			Assert.IsFalse(r2.Intersects(r1));
		}

		[Test]
		public void TestPoints() {
			var r1 = new Rectangle(1, 1, 6, 6);


			foreach (var p in r1) {
				Assert.IsTrue(r1.Contains(p));
			}

			Assert.AreEqual(r1.Count(), r1.Width * r1.Height);

			var r2 = new Rectangle(0, 0, 0, 0);

			Assert.AreEqual(r2.Count(), 0);
		}

		[Test]
		public void TestInflate() {
			var r = new Rectangle(0, 0, 6, 6);
			var inflatedRect = r.Inflate(3, 3);

			Assert.AreEqual(inflatedRect.X, -3);
			Assert.AreEqual(inflatedRect.Y, -3);

			Assert.AreEqual(inflatedRect.Width, 12);
			Assert.AreEqual(inflatedRect.Height, 12);

			var reducedRect = r.Inflate(-2, -2);

			Assert.AreEqual(reducedRect.X, 2);
			Assert.AreEqual(reducedRect.Y, 2);

			Assert.AreEqual(reducedRect.Width, 2);
			Assert.AreEqual(reducedRect.Height, 2);
		}

		[Test]
		public void TestLargeNegativeInflation() {
			var r = new Rectangle(0, 0, 6, 6);
			var deflated = r.Inflate(-6, -6);

			Assert.AreEqual(deflated.Width, -6);
			Assert.AreEqual(deflated.Height, -6);
			Assert.AreEqual(deflated.Top, 6);
			Assert.AreEqual(deflated.Bottom, 0);
		}

		#region Public static properties

		[Test]
		public void TestEmpty() {
			Rectangle r = Rectangle.Empty;

			Assert.AreEqual(0, r.X);
			Assert.AreEqual(0, r.Y);
			Assert.AreEqual(0, r.Size.Width);
			Assert.AreEqual(0, r.Size.Height);
		}

		#endregion

		#region Constructors

		[Test]
		public void TestConstructorDefault() {
			Rectangle r = new Rectangle();

			Assert.AreEqual(0, r.X);
			Assert.AreEqual(0, r.Y);
			Assert.AreEqual(0, r.Size.Width);
			Assert.AreEqual(0, r.Size.Height);
		}

		[Test]
		public void TestConstructorSizeSize() {
			Rectangle r = new Rectangle(new Size(3, 5));

			Assert.AreEqual(0, r.X);
			Assert.AreEqual(0, r.Y);
			Assert.AreEqual(3, r.Size.Width);
			Assert.AreEqual(5, r.Size.Height);
		}

		[Test]
		public void TestConstructorPositionPointSizeSize() {
			Rectangle r = new Rectangle(new Point(2, 4), new Size(3, 5));

			Assert.AreEqual(2, r.X);
			Assert.AreEqual(4, r.Y);
			Assert.AreEqual(3, r.Size.Width);
			Assert.AreEqual(5, r.Size.Height);
		}

		[Test]
		public void TestConstructorPositionIntSizeInt() {
			Rectangle r = new Rectangle(2, 4, 3, 5);

			Assert.AreEqual(2, r.X);
			Assert.AreEqual(4, r.Y);
			Assert.AreEqual(3, r.Size.Width);
			Assert.AreEqual(5, r.Size.Height);
		}

		[Test]
		public void TestConstructorPositionPointSizeInt() {
			Rectangle r = new Rectangle(new Point(2, 4), 3, 5);

			Assert.AreEqual(2, r.X);
			Assert.AreEqual(4, r.Y);
			Assert.AreEqual(3, r.Size.Width);
			Assert.AreEqual(5, r.Size.Height);
		}

		[Test]
		public void TestConstructorPositionIntSizeSize() {
			Rectangle r = new Rectangle(2, 4, new Size(3, 5));

			Assert.AreEqual(2, r.X);
			Assert.AreEqual(4, r.Y);
			Assert.AreEqual(3, r.Size.Width);
			Assert.AreEqual(5, r.Size.Height);
		}

		#endregion

		#region Enumeration

		[Test]
		public void TestEnumerateEmpty() {
			TestEnumeration(Rectangle.Empty);
		}

		[Test]
		public void TestEnumerateZeroWidth() {
			TestEnumeration(new Rectangle(-3, 2, 0, 10));
		}

		[Test]
		public void TestEnumerateZeroHeight() {
			TestEnumeration(new Rectangle(3, -2, 10, 0));
		}

		[Test]
		public void TestEnumerate() {
			TestEnumeration(new Rectangle(4, 5, 3, 2),
			                new Point(4, 5),
			                new Point(5, 5),
			                new Point(6, 5),
			                new Point(4, 6),
			                new Point(5, 6),
			                new Point(6, 6));
		}

		#endregion

		[Test]
		public void Contains() {
			// identical Rectangle is inside
			Assert.IsTrue(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(0, 0, 3, 4)));

			// zero size Rectangle can still be inside
			Assert.IsTrue(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(1, 2, 0, 0)));

			// outer corners of Rectangle are included
			Assert.IsTrue(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(0, 0, 0, 0)));
			Assert.IsTrue(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(3, 4, 0, 0)));

			// point must be in
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(-1, 0, 0, 0)));

			// off left side
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(-1, 1, 2, 2)));

			// off right side
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(5, 1, 2, 2)));

			// off top side
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(1, -3, 2, 2)));

			// off bottom side
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(1, 5, 2, 2)));

			// completely surrounded
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(-1, -1, 5, 6)));

			// off two sides
			Assert.IsFalse(new Rectangle(0, 0, 3, 4).Contains(new Rectangle(-1, 1, 6, 2)));
		}

		[Test]
		public void TestCoordinates() {
			Rectangle rect = new Rectangle(1, 2, 3, 4);

			// x, y
			Assert.AreEqual(1, rect.X);
			Assert.AreEqual(2, rect.Y);

			// size
			Assert.AreEqual(3, rect.Width);
			Assert.AreEqual(4, rect.Height);

			// ltrb
			Assert.AreEqual(1, rect.Left);
			Assert.AreEqual(2, rect.Top);
			Assert.AreEqual(1 + 3, rect.Right);
			Assert.AreEqual(2 + 4, rect.Bottom);

			// ltrb vecs
			Assert.AreEqual(new Point(1, 2), rect.TopLeft);
			Assert.AreEqual(new Point(1 + 3, 2), rect.TopRight);
			Assert.AreEqual(new Point(1, 2 + 4), rect.BottomLeft);
			Assert.AreEqual(new Point(1 + 3, 2 + 4), rect.BottomRight);
		}

		private void TestEnumeration(IEnumerable<Point> enumerable, params Point[] expected) {
			// build the queue of expected vectors
			List<Point> list = expected.ToList();

			// enumerate and compare
			foreach (Point pos in enumerable) {
				CollectionAssert.Contains(list, pos);
				list.Remove(pos);
			}

			// make sure we got as many as expected
			Assert.AreEqual(0, list.Count);
		}
	}
}
