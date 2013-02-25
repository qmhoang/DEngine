using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Extensions;
using NUnit.Framework;

namespace DEngineTests.Extensions {
	[TestFixture]
	class EnumerableExtensionTests {
		public IEnumerable EmptyCases {
			get {
				yield return new TestCaseData(new decimal[] {}).Returns(true);
				yield return new TestCaseData(new int[] {0}).Returns(false);
				yield return new TestCaseData(new double[] {}).Returns(true);
				yield return new TestCaseData(new bool[] {false}).Returns(false);
				yield return new TestCaseData(new List<decimal>()).Returns(true);
				yield return new TestCaseData(new List<string>{""}).Returns(false);
				yield return new TestCaseData(new Dictionary<int, dynamic> {{1, null}}).Returns(false);
			}
		}

		[TestCaseSource("EmptyCases")]
		public bool TestIsEmpty(IEnumerable enumeration) {
			return enumeration.IsEmpty();
		}
	}
}
