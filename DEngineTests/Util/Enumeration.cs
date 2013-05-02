using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests.Util {
	public static class Enumeration {
		public static void TestEnumeration(IEnumerable<Point> enumerable, params Point[] expected) {
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
