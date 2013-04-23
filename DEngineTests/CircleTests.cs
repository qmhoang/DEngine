using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class CircleTests {
		public static IEnumerable CircleCases {
			get {
				yield return new TestCaseData(new Point(1, 2), new Circle(new Point(0, 0), 3)).Returns(true);
				yield return new TestCaseData(new Point(3, 2), new Circle(new Point(0, 0), 3)).Returns(false);
				yield return new TestCaseData(new Point(111, 222), new Circle(new Point(333, 444), 300)).Returns(false);
				yield return new TestCaseData(new Point(111, 222), new Circle(new Point(333, 444), 543)).Returns(true);
				yield return new TestCaseData(new Point(0, 0), new Circle(new Point(0, 0), 0)).Returns(true);
			}
		}

		[TestCaseSource("CircleCases")]
		public static bool TestInCircle(Point p, Circle c) {
			return c.Contains(p);			
		}

		[Test]
		public void TestEnumerateEmpty() {
			TestEnumeration(new Circle(0, 0, 0), new Point(0, 0));
		}

		[Test]
		public void TestEnumerateZeroRadius() {
			TestEnumeration(new Circle(-5, 10, 0),new Point(-5, 10));
		}

		[Test]
		public void TestEnumerate() {
			TestEnumeration(new Circle(4, 5, 2),
							new Point(4, 3),

							new Point(3, 4),
							new Point(4, 4),
							new Point(5, 4),

							new Point(2, 5),
							new Point(3, 5),
							new Point(4, 5),
							new Point(5, 5),
							new Point(6, 5),

							new Point(3, 6),
							new Point(4, 6),
							new Point(5, 6),
							
							new Point(4, 7));
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