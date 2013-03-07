using System.Collections;
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
	}
}