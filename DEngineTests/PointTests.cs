using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
    [TestFixture]
    class PointTests {
        [Test]
        public static void TestCreationAndAccess() {
            Point p = new Point(1, 2);
            Assert.IsTrue(p.X == 1);
            Assert.IsTrue(p.Y == 2);

            p.X = 333;
            p.Y = 444;
            Assert.IsTrue(p.X == 333 && p.Y == 444);
        }

        [Test]
        public static void TestMath() {
            Point p1 = new Point(111, 222);
            Point p2 = new Point(333, 444);

            Point r1 = p1 + p2;
            Assert.IsTrue(r1.X == 444 && r1.Y == 666);

            Point r2 = p1 - p2;
            Assert.IsTrue(r2.X == -222 && r2.Y == -222);

            Assert.IsTrue(p1 * 3 == new Point(333, 666));
            Assert.IsFalse(p2 * -1 == Point.Zero);
        }

        [Test]
        public static void TestDirectionsAndComparison() {
            Assert.IsTrue(Point.Zero == new Point(0, 0));
            Assert.IsTrue(Point.One == new Point(1, 1));
            Assert.IsTrue(Point.North == new Point(0, -1));
            Assert.IsTrue(Point.South == new Point(0, 1));
            Assert.IsTrue(Point.West == new Point(-1, 0));
            Assert.IsTrue(Point.East == new Point(1, 0));

            Assert.IsTrue(Point.North + Point.South == Point.Zero);
            Assert.IsTrue(Point.East + Point.West == Point.Zero);

            Assert.IsTrue(Point.Southeast + Point.Northwest == Point.Zero);
            Assert.IsTrue(Point.Southwest + Point.Northeast == Point.Zero);

            Assert.IsTrue(Point.North + Point.East == Point.Northeast);
            Assert.IsTrue(Point.North + Point.West == Point.Northwest);
            Assert.IsTrue(Point.South + Point.East == Point.Southeast);
            Assert.IsTrue(Point.South + Point.West == Point.Southwest);
        }

        [Test]
        public static void TestLength() {
            Assert.IsTrue(new Point(3, 4).Length() == 5);
            Assert.IsTrue(new Point(3, 6).DistanceTo(new Point(7, 3)) == 5);
            Assert.IsTrue(new Point(2, 3).DistanceTo(Point.Zero) > 3);
        }

        [Test]
        public static void TestLeftRight() {
            Assert.IsTrue(Point.North.Left == Point.West);
            Assert.IsTrue(Point.North.Right == Point.East);
        }

        [Test]
        public static void TestInCircle() {
            Assert.IsTrue(new Point(1, 2).IsInCircle(Point.Zero, 3));
            Assert.IsFalse(new Point(3, 2).IsInCircle(Point.Zero, 3));

            //            Assert.IsTrue(Position.Zero.IsInRectangle(Position.Northeast, Position.Southwest));
        }
    }
}
