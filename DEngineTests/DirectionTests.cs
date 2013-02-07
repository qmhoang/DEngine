using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class DirectionTests {
		[Test]
		public void TestEqual() {
			Assert.IsTrue(Direction.North == new Point(0, -1));
			Assert.IsTrue(Direction.South == new Point(0, 1));
			Assert.IsTrue(Direction.West == new Point(-1, 0));
			Assert.IsTrue(Direction.East == new Point(1, 0));
		}

		[Test]
		public static void TestRotations() {
			Assert.AreEqual(Direction.North.RotateLeft90, Direction.West);
			Assert.AreEqual(Direction.North.RotateRight90, Direction.East);

			Assert.AreEqual(Direction.West.RotateRight90, Direction.North);
			Assert.AreEqual(Direction.West.RotateLeft90, Direction.South);

			Assert.AreEqual(Direction.North.Rotate180, Direction.South);
		}

	}
}