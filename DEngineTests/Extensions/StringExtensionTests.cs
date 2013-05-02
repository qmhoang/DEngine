using System;
using DEngine.Extensions;
using NUnit.Framework;

namespace DEngineTests.Extensions {
	[TestFixture]
	public class StringExtensionTests {
		// todo: need to test will low linewidth
		[TestCase("this is a string that we will try to wrap around", 10, Result = new string[] { "this is a", "string", "that we", "will try", "to wrap", "around" })]
		[TestCase("", 1, Result = new string[] { "" })]
		public string[] TestWordWrap(string text, int lineWidth) {
			var v = text.WordWrap(lineWidth);
			return v;
		}

		[Test]
		public void TestMethod() {
			var r = "this is".WordWrap(-1);
			var r0 = "this is".WordWrap(0);
			var r1 = "this is".WordWrap(1);
			var r2 = "this is".WordWrap(2);

			var r3 = "I am not affiliated with Bioware, Black Isle, Interplay or anyone who had".WordWrap(10);
			Console.WriteLine(r);
		}
	}
}