using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class RectTests {
		[Test]
		public void EmptyConstructor() {
			var r = new Rect();
			Assert.AreEqual(0, r.X);
			Assert.AreEqual(0, r.Y);
			Assert.AreEqual(0, r.Width);
			Assert.AreEqual(0, r.Height);

			Assert.AreEqual(0, r.Center.X);
			Assert.AreEqual(0, r.Center.Y);
		}

		[Test]
		public void FilledConstructor() {
			var r = new Rect(15, 25, 35, 45);
			Assert.AreEqual(15, r.X);
			Assert.AreEqual(25, r.Y);
			Assert.AreEqual(35, r.Width);
			Assert.AreEqual(45, r.Height);
		}

		[Test]
		public void Edges() {
			var r = new Rect(0, 0, 2, 2);
			Assert.AreEqual(0, r.Top);
			Assert.AreEqual(0, r.Left);
			Assert.AreEqual(2, r.Right);
			Assert.AreEqual(2, r.Bottom);
		}

		[Test]
		public void EdgesNegative() {
			var r = new Rect(-15, -15, 30, 30);
			Assert.AreEqual(-15, r.Top);
			Assert.AreEqual(-15, r.Left);
			Assert.AreEqual(15, r.Right);
			Assert.AreEqual(15, r.Bottom);
		}

		[Test]
		public void Center() {
			var r = new Rect(-1, -1, 2, 2); // 2x2, centered on 0,0
			Assert.AreEqual(0, r.Center.X);
			Assert.AreEqual(0, r.Center.Y);
		}

		[Test]
		public void OffCenter() {
			var r = new Rect(0, 0, 2, 2);
			Assert.AreEqual(1, r.Center.X);
			Assert.AreEqual(1, r.Center.Y);
		}

		[Test]
		public void ContainsRect() {
			var outer = new Rect(0, 0, 10, 10);
			var inner = new Rect(5, 5, 2, 2);

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public void EmptyContainsRect() {
			var outer = new Rect(-1, -1, 2, 2);
			var inner = new Rect();

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public void ContainsVector() {
			var outer = new Rect(0, 0, 10, 10);
			var inner = new Point(5, 5);

			Assert.IsTrue(outer.Contains(inner));
		}

		[Test]
		public void DoesNotContainRect() {
			var outer = new Rect(0, 1, 2, 2);
			var inner = new Rect(0, 0, 2, 2);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public void DoesNotContainPoint() {
			var outer = new Rect(0, 0, 1, 1);
			var inner = new Point(5, 5);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public void EmptyDoesNotContainPoint() {
			var outer = new Rect();
			var inner = new Point(5, 5);

			Assert.IsFalse(outer.Contains(inner));
		}

		[Test]
		public void EmptyIntersects() {
			var outer = new Rect();
			var inner = new Rect();

			Assert.IsTrue(outer.Intersects(inner));
		}

		[Test]
		public void Intersects() {
			var outer = new Rect(0, 0, 20, 20);
			var inner = new Rect(05, 05, 20, 20);

			Assert.IsTrue(outer.Intersects(inner));
		}

		[Test]
		public void DoesNotIntersect() {
			var outer = new Rect(0, 0, 2, 2);
			var inner = new Rect(5, 5, 2, 2);

			Assert.IsTrue(outer.Intersects(inner));
		}
	}
}
