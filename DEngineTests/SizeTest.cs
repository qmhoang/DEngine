using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class SizeTest {
		[Test]
		public void TestArea() {
			Assert.AreEqual(0, new Size(0, 0).Area);
			Assert.AreEqual(0, new Size(1, 0).Area);
			Assert.AreEqual(1, new Size(1, -1).Area);
			Assert.AreEqual(4, new Size(-2, -2).Area);
			Assert.AreEqual(6, new Size(2, 3).Area);
		} 
	}
}