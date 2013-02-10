using System.Collections.Concurrent;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	class AbstractWorldTest {
		
		[Test]
		public static void TestAPFunctions() {
			// an action that takes 2 second should equal 200  AP
			Assert.AreEqual(AbstractWorld.SecondsToActionPoints(2), 200);

			Assert.AreEqual(AbstractWorld.SecondsToSpeed(10), 10);

			Assert.AreEqual(AbstractWorld.SecondsToSpeed(AbstractWorld.SpeedToSeconds(5)), 5);
			Assert.AreEqual(AbstractWorld.SecondsToSpeed(AbstractWorld.SpeedToSeconds(9)), 9);
			Assert.AreEqual(AbstractWorld.SecondsToSpeed(AbstractWorld.SpeedToSeconds(50)), 50);
			Assert.AreEqual(AbstractWorld.SecondsToSpeed(AbstractWorld.SpeedToSeconds(14)), 14);
		}

		[TestCase(100)]
		[TestCase(50)]
		[TestCase(200)]
		[TestCase(150)]
		public void TestStaticConversion(int value) {
			Assert.AreEqual(AbstractWorld.SecondsToSpeed(AbstractWorld.SpeedToSeconds(value)), value);
			Assert.AreEqual(AbstractWorld.SecondsToActionPoints(AbstractWorld.SpeedToSeconds(AbstractWorld.ActionPointsToSpeed(value))), value);
		}
	}
}
