using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DEngine.Core;
using NUnit.Framework;

namespace RoguelikeTests {
    [TestFixture]
    class DiceTesting {
        [Test]
        public static void TestDiceCreation() {
            Dice d = new Dice(1, 2, 3, 4);
            Assert.IsTrue(d.Min == 16 && d.InclusiveMax == 20);
            Dice e = new Dice(6);
            Assert.IsTrue(e.Min == 0 && e.InclusiveMax == 6 && e.ExclusiveMax == 7);
        }

        [Test]
        public static void TestDiceMinMax() {
            Dice d1 = new Dice(3, 6);
            Assert.IsTrue(d1.Min == 3 && d1.InclusiveMax == 18);

            Dice d2 = new Dice(3, 6, 2);
            Assert.IsTrue(d2.Min == 5 && d2.InclusiveMax == 20);

            Dice d3 = new Dice(3, 6, 2, 2);
            Assert.IsTrue(d3.Min == 10 && d3.InclusiveMax == 40);
        }

        [Test]
        public static void TestParsing() {
            Dice d1 = Dice.Parse("1d6");
            Assert.IsTrue(d1.Min == 1 && d1.InclusiveMax == 6);

            Dice d2 = Dice.Parse("2d10 + 2");
            Assert.IsTrue(d2.Min == 4 && d2.InclusiveMax == 22);

            Dice d3 = Dice.Parse("3d4 + 1 * 2");
            Assert.IsTrue(d3.Min == 8 && d3.InclusiveMax == 26);

            Dice d4 = Dice.Parse("3-18");
            Assert.IsTrue(d4.Min == 3 && d4.InclusiveMax == 18);
        }

        [Test]
        public static void TestEquality() {
            Assert.IsTrue(Dice.Parse("1d8") == Dice.Parse("1-8"));
            Assert.IsTrue(new Dice(1, 10, 1) == Dice.Parse("1d10+1"));
        }

        [Test]
        public static void TestRolling() {
            Dice d = new Dice(1, 8);
            for (int i = 0; i < 1000; i++) {
                int r = d.Roll();
                Assert.IsTrue(r >= d.Min && r <= d.InclusiveMax);
            }
            Assert.IsTrue(d.RollMax() == 8);
        }
    }
}
