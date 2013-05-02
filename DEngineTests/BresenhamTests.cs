using System.Collections.Generic;
using System.Linq;
using DEngine.Core;
using DEngine.Level;
using DEngineTests.Util;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class BresenhamTests {
		[Test]
		public void TestLine() {
			Enumeration.TestEnumeration(Bresenham.GeneratePointsFromLine(new Point(0, 0), new Point(0, 10)),
			                            new Point(0, 0),
			                            new Point(0, 1),
			                            new Point(0, 2),
			                            new Point(0, 3),
			                            new Point(0, 4),
			                            new Point(0, 5),
			                            new Point(0, 6),
			                            new Point(0, 7),
			                            new Point(0, 8),
			                            new Point(0, 9),
			                            new Point(0, 10));

			Enumeration.TestEnumeration(Bresenham.GeneratePointsFromLine(new Point(0, 10), new Point(0, 0)),
			                            new Point(0, 10),
			                            new Point(0, 9),
			                            new Point(0, 8),
			                            new Point(0, 7),
			                            new Point(0, 6),
			                            new Point(0, 5),
			                            new Point(0, 4),
			                            new Point(0, 3),
			                            new Point(0, 2),
			                            new Point(0, 1),
			                            new Point(0, 0));

		}

		[Test]
		public void TestLineIgnoreOrigin() {
			Enumeration.TestEnumeration(Bresenham.GeneratePointsFromLine(new Point(0, 0), new Point(0, 10), false),
			                            new Point(0, 1),
			                            new Point(0, 2),
			                            new Point(0, 3),
			                            new Point(0, 4),
			                            new Point(0, 5),
			                            new Point(0, 6),
			                            new Point(0, 7),
			                            new Point(0, 8),
			                            new Point(0, 9),
			                            new Point(0, 10));
		}

		[Test]
		public void TestOriginEndSame() {
			Enumeration.TestEnumeration(Bresenham.GeneratePointsFromLine(new Point(0, 0), new Point(0, 0), false));
			Enumeration.TestEnumeration(Bresenham.GeneratePointsFromLine(new Point(0, 0), new Point(0, 0)), new Point(0, 0));
		}
	}
}