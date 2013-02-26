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
		public static void TestEmptyConstructor() {
			var r = new Rectangle();
			Assert.AreEqual(0, r.TopLeft.X);
			Assert.AreEqual(0, r.TopLeft.Y);
			Assert.AreEqual(0, r.Width);
			Assert.AreEqual(0, r.Height);

			Assert.AreEqual(0, r.Center.X);
			Assert.AreEqual(0, r.Center.Y);
		}

		[Test]
		public static void TestFilledConstructor() {
			var r = new Rectangle(15, 25, 35, 45);
			Assert.AreEqual(15, r.TopLeft.X);
			Assert.AreEqual(25, r.TopLeft.Y);
			Assert.AreEqual(35, r.Width);
			Assert.AreEqual(45, r.Height);
		}

		[Test]
		public static void TestConstructorEquality() {
			var r1 = new Rectangle(0, 0, 2, 2);
			var r2 = new Rectangle(new Point(0, 0), new Size(2, 2));
			var r3 = new Rectangle(new Point(0, 0), new Point(2, 2));

			Assert.AreEqual(r1, r2);
			Assert.AreEqual(r2, r3); // by transitive r1 = r3 also
		}

		[Test]
		public static void TestEdges() {
			var r = new Rectangle(0, 0, 2, 2);
			Assert.AreEqual(0, r.Top);
			Assert.AreEqual(0, r.Left);
			Assert.AreEqual(2, r.Right);
			Assert.AreEqual(2, r.Bottom);			
		}

		[Test]
		public static void TestEdgesNegative() {
			var r = new Rectangle(-15, -15, 30, 30);
			Assert.AreEqual(-15, r.Top);
			Assert.AreEqual(-15, r.Left);
			Assert.AreEqual(15, r.Right);
			Assert.AreEqual(15, r.Bottom);
		}

		[Test]
		public static void TestCenter() {
			var r = new Rectangle(-1, -1, 2, 2); // 2x2, centered on 0,0
			Assert.AreEqual(0, r.Center.X);
			Assert.AreEqual(0, r.Center.Y);
		}

		[Test]
		public static void TestOffCenter() {
			var r = new Rectangle(0, 0, 2, 2);
			Assert.AreEqual(1, r.Center.X);
			Assert.AreEqual(1, r.Center.Y);

			r = new Rectangle(0, 0, 3, 3);
			Assert.AreEqual(1, r.Center.X);
			Assert.AreEqual(1, r.Center.Y);
		}

		[Test]
		public static void TestContainsRect() {
			var outer = new Rectangle(0, 0, 10, 10);
			var inner = new Rectangle(5, 5, 2, 2);

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public static void TestEmptyContainsRect() {
			var outer = new Rectangle(-1, -1, 2, 2);
			var inner = new Rectangle();

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public static void TestContainsPoint(
									[Random(0, 99, 4)] int px,
									[Random(0, 99, 4)] int py) {
			var outer = new Rectangle(0, 0, 100, 100);
			var point = new Point(px, py);

			Assert.IsTrue(outer.Contains(point));
		}

		[Test]
		public static void TestDoesNotContainRect() {
			var outer = new Rectangle(0, 1, 2, 2);
			var inner = new Rectangle(0, 0, 2, 2);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public static void TestDoesNotContainPoint() {
			var outer = new Rectangle(0, 0, 1, 1);
			var inner = new Point(5, 5);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public static void TestEmptyDoesNotContainPoint() {
			var outer = new Rectangle();
			var inner = new Point(5, 5);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public static void TestEmptyDoesNotContainOrigin() {
			var outer = new Rectangle();
			var inner = new Point();

			Assert.IsFalse(outer.Contains(inner));		// matches java.awt.Rectangle and System.Drawing.Rectangle behavior
		}

		[Test]
		public static void TestEmptyIntersectsFails() {
			var outer = new Rectangle();
			var inner = new Rectangle();

			Assert.IsFalse(outer.Intersects(inner));	// matches java.awt.Rectangle and System.Drawing.Rectangle behavior
		}

		[Test]
		public static void TestIntersects() {
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
		public static void TestDoesNotIntersect() {
			var r1 = new Rectangle(0, 0, 2, 2);
			var r2 = new Rectangle(5, 5, 2, 2);

			Assert.IsFalse(r1.Intersects(r2));
			Assert.IsFalse(r2.Intersects(r1));

			r1 = new Rectangle(0, 0, 2, 2);
			r2 = new Rectangle(2, 2, 1, 1);

			Assert.IsFalse(r1.Intersects(r2));
			Assert.IsFalse(r2.Intersects(r1));
		}
	}
}
