using System;
using System.Diagnostics;
using System.Timers;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	class RngTests {
		[SetUp]
		public static void SetUp() {
			Rng.Seed(0);
		}
//		[Test]
//		public static void TestDiceCreation() {
//			Rand d = new Rand(1, 2, 3, 4);
//			Assert.IsTrue(Math.Abs(d.Mininum - 16) < float.Epsilon && d.InclusiveMax == 20);
//			Rand e = new Rand(6);
//			Assert.IsTrue(e.Mininum == 0 && e.InclusiveMax == 6 && e.ExclusiveMax == 7);
//		}
//
//		[Test]
//		public static void TestDiceMinMax() {
//			Rand d1 = new Rand(3, 6);
//			Assert.IsTrue(d1.Mininum == 3 && d1.InclusiveMax == 18);
//
//			Rand d2 = new Rand(3, 6, 2);
//			Assert.IsTrue(d2.Mininum == 5 && d2.InclusiveMax == 20);
//
//			Rand d3 = new Rand(3, 6, 2, 2);
//			Assert.IsTrue(d3.Mininum == 10 && d3.InclusiveMax == 40);
//		}
//
//		[Test]
//		public static void TestParsing() {
//			Rand d1 = Rand.ParseDice("1d6");
//			Assert.IsTrue(d1.Mininum == 1 && d1.InclusiveMax == 6);
//
//			Rand d2 = Rand.ParseDice("2d10 + 2");
//			Assert.IsTrue(d2.Mininum == 4 && d2.InclusiveMax == 22);
//
//			Rand d3 = Rand.ParseDice("3d4 + 1 * 2");
//			Assert.IsTrue(d3.Mininum == 8 && d3.InclusiveMax == 26);
//
//			Rand d4 = Rand.ParseDice("3-18");
//			Assert.IsTrue(d4.Mininum == 3 && d4.InclusiveMax == 18);
//		}
//
//		[Test]
//		public static void TestEquality() {
//			Assert.IsTrue(Rand.ParseDice("1d8") == Rand.ParseDice("1-8"));
//			Assert.IsTrue(new Rand(1, 10, 1) == Rand.ParseDice("1d10+1"));
//		}
//
//		[Test]
//		public static void TestRolling() {
//			Rand d = new Rand(1, 8);
//			for (int i = 0; i < 1000; i++) {
//				int r = d.Roll();
//				Assert.IsTrue(r >= d.Mininum && r <= d.InclusiveMax);
//			}
//			Assert.IsTrue(d.RollMax() == 8);
//		}

		[Test]
		public static void TestTriangle() {
			int[] list = new int[20];

			for (int i = 0; i < 100000; i++) {
				list[Rng.TriangleInt(10, 8)]++;
			}

			int index = 0;
			foreach (var i in list) {
				Console.WriteLine("{0}: {1}", index, i);
				index++;
			}
		}

		[Test]
		public static void TestGaussian() {

			int[] list = new int[51];

			for (int i = 0; i < 400000; i++) {
				int j = Rng.GaussianInt(10, 10, 3);
//                Console.WriteLine(j);
				list[j]++;
			}
			
			int index = 0;
			foreach (var i in list) {
				Console.WriteLine("{0}: {1}", index, i);
				index++;
			}
		}

		[Test]
		public static void TestAddition() {
			var r = Rand.Constant(10) + Rand.Constant(10) + Rand.Constant(55);

			Assert.AreEqual(75, r.Roll());
		}
	}
}
