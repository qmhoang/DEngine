using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class DirectionTests {
		[Test]
		public void TestEquality() {
			Assert.IsTrue(Direction.North == new Point(0, -1));
			Assert.IsTrue(Direction.South == new Point(0, 1));
			Assert.IsTrue(Direction.West == new Point(-1, 0));
			Assert.IsTrue(Direction.East == new Point(1, 0));
		}

		[Test]
		public void TestRotations() {
			Assert.AreEqual(Direction.North.RotateLeft90, Direction.West);
			Assert.AreEqual(Direction.North.RotateRight90, Direction.East);

			Assert.AreEqual(Direction.West.RotateRight90, Direction.North);
			Assert.AreEqual(Direction.West.RotateLeft90, Direction.South);

			Assert.AreEqual(Direction.North.Rotate180, Direction.South);
		}

		[Test]
		public void TestClockwise() {
			Assert.AreEqual(Direction.North.Counterclockwise, Direction.NW);
			Assert.AreEqual(Direction.North.Clockwise, Direction.NE);

			Assert.AreEqual(Direction.West.Clockwise, Direction.NW);
			Assert.AreEqual(Direction.West.Counterclockwise, Direction.SW);

			Assert.AreEqual(Direction.North.Clockwise.Counterclockwise, Direction.N);
		}

		[Test]
		public void TestImplicits() {
			Direction north = new Point(0, -1);
			Assert.AreEqual(north, Direction.N);

			Point south = Direction.S;
			Assert.AreEqual(south, new Point(0, 1));

			Direction west = new Point(-5, 0);
			Assert.AreEqual(west, Direction.W);
		}

		[Test]
		public void TestConstruct() {
			Direction d = new Direction();

			Assert.AreEqual(d, Direction.None);
		}

		[Test]
		public void TestTorwards() {
			Point p = new Point(6, 1);
			var d = Direction.Towards(p);

			Assert.AreEqual(d, Direction.SE);

			Assert.AreEqual(d.Offset, new Point(1, 1));
		}
	}
}