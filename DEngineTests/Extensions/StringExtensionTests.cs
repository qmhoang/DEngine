using DEngine.Extensions;
using NUnit.Framework;

namespace DEngineTests.Extensions {
	[TestFixture]
	public class StringExtensionTests {
		// todo: need to test will low linewidth
		[TestCase("this is a string that we will try to wrap around", 10, Result = new string[] {"this is a", "string", "that we", "will try", "to wrap", "around"})]
		public string[] TestWordWrap(string text, int lineWidth) {
			var v = text.WordWrap(lineWidth);
			return v;
		}
	}
}