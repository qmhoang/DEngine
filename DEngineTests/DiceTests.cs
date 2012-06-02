using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
    [TestFixture]
    class DiceTesting {
        [Test]
        public static void TestDiceCreation() {
            Range d = new Range(1, 2, 3, 4);
            Assert.IsTrue(d.Min == 16 && d.InclusiveMax == 20);
            Range e = new Range(6);
            Assert.IsTrue(e.Min == 0 && e.InclusiveMax == 6 && e.ExclusiveMax == 7);
        }

        [Test]
        public static void TestDiceMinMax() {
            Range d1 = new Range(3, 6);
            Assert.IsTrue(d1.Min == 3 && d1.InclusiveMax == 18);

            Range d2 = new Range(3, 6, 2);
            Assert.IsTrue(d2.Min == 5 && d2.InclusiveMax == 20);

            Range d3 = new Range(3, 6, 2, 2);
            Assert.IsTrue(d3.Min == 10 && d3.InclusiveMax == 40);
        }

        [Test]
        public static void TestParsing() {
            Range d1 = Range.ParseDice("1d6");
            Assert.IsTrue(d1.Min == 1 && d1.InclusiveMax == 6);

            Range d2 = Range.ParseDice("2d10 + 2");
            Assert.IsTrue(d2.Min == 4 && d2.InclusiveMax == 22);

            Range d3 = Range.ParseDice("3d4 + 1 * 2");
            Assert.IsTrue(d3.Min == 8 && d3.InclusiveMax == 26);

            Range d4 = Range.ParseDice("3-18");
            Assert.IsTrue(d4.Min == 3 && d4.InclusiveMax == 18);
        }

        [Test]
        public static void TestEquality() {
            Assert.IsTrue(Range.ParseDice("1d8") == Range.ParseDice("1-8"));
            Assert.IsTrue(new Range(1, 10, 1) == Range.ParseDice("1d10+1"));
        }

        [Test]
        public static void TestRolling() {
            Range d = new Range(1, 8);
            for (int i = 0; i < 1000; i++) {
                int r = d.Roll();
                Assert.IsTrue(r >= d.Min && r <= d.InclusiveMax);
            }
            Assert.IsTrue(d.RollMax() == 8);
        }
    }
}
