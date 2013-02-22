using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Extensions;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	internal class IteratorTests {
		[Test]
		public void TestIterator() {
			List<int> lst = new List<int>
			                {
			                		1,
			                		2,
			                		3,
			                		4,
			                		5,
			                		6,
			                		7
			                };

			var iter = lst.GetIterator();
			int i = 0;
			while (iter.HasNext) {
				Assert.AreEqual(iter.Next(), lst[i]);
				i++;
			}
		}

		[Test]
		[ExpectedException(typeof(NoSuchElementException))]
		public void TestNoSuchElement() {
			List<int> lst = new List<int>
			                {
			                		1
			                };

			var iter = lst.GetIterator();

			iter.Next(); // fine
			iter.Next(); // throws
		}
	}
}
