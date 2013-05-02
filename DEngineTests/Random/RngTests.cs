using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DEngine.Core;
using DEngine.Random;
using DEngineTests.Util;
using NUnit.Framework;

namespace DEngineTests.Random {
	[TestFixture]
	public class RngTests {
		#region Seed

		[Test]
		public void TestSeed() {
			for (int seed = 0; seed < 100; seed++) {
				// get a seeded sequence
				Queue<int> results = new Queue<int>();

				Rng.Seed(seed);

				for (int i = 0; i < 10; i++) {
					results.Enqueue(Rng.Int(100));
				}

				// reseed and make sure we get the same results
				Rng.Seed(seed);

				while (results.Count > 0) {
					Assert.AreEqual(results.Dequeue(), Rng.Int(100));
				}
			}
		}

		#endregion

		#region Int

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestIntMaxThrowsIfNegative() {
			Rng.Int(-2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestIntMaxThrowsIfZero() {
			Rng.Int(0);
		}

		[TestCase(1, new[] {1.0f})]
		[TestCase(2, new[] {0.5f, 0.5f})]
		[TestCase(5, new[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f})]
		public void TestIntMax(int max, float[] expected) {
			Statistics.Frequencies(expected, () => Rng.Int(max));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestIntMinMaxThrowsBadRange() {
			Rng.Int(5, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestIntMinMaxThrowsIfMaxEqualsMin() {
			Rng.Int(5, 5);
		}

		[Test]
		public void TestIntMinMax() {
			Statistics.Frequencies(new[] {1.0f}, () => Rng.Int(3, 4) - 3);
			Statistics.Frequencies(new[] {0.5f, 0.5f}, () => Rng.Int(-4, -2) + 4);
			Statistics.Frequencies(new[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => Rng.Int(4, 9) - 4);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestIntInclusiveMaxThrowsIfNegative() {
			Rng.IntInclusive(-2);
		}

		[TestCase(0, new[] {1.0f})]
		[TestCase(1, new[] {0.5f, 0.5f})]
		[TestCase(3, new[] {0.25f, 0.25f, 0.25f, 0.25f})]
		[TestCase(4, new[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f})]
		public void TestIntInclusiveMax(int max, float[] expected) {
			Statistics.Frequencies(expected, () => Rng.IntInclusive(max));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestIntInclusiveMinMaxThrowsBadRange() {
			Rng.IntInclusive(5, 3);
		}

		[Test]
		public void TestIntInclusiveMinMax() {
			Statistics.Frequencies(new[] {1.0f}, () => Rng.IntInclusive(5, 5) - 5);
			Statistics.Frequencies(new[] {0.5f, 0.5f}, () => Rng.IntInclusive(3, 4) - 3);
			Statistics.Frequencies(new[] {0.25f, 0.25f, 0.25f, 0.25f}, () => Rng.IntInclusive(-5, -2) + 5);
			Statistics.Frequencies(new[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => Rng.IntInclusive(4, 8) - 4);
		}

		#endregion

		#region Float

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestFloatMaxThrowsIfNegative() {
			Rng.Double(-2);
		}

		[Test]
		public void TestFloatMax() {
			Statistics.Frequencies(new[] {1.0f}, () => (int) Rng.Double(0));
			Statistics.Frequencies(new[] {1.0f}, () => (int) Rng.Double(1));
			Statistics.Frequencies(new[] {0.5f, 0.5f}, () => (int) Rng.Double(2));
			Statistics.Frequencies(new[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => (int) (Rng.Double(0.5f) * 10.0f));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestFloatMinMaxThrowsBadRange() {
			Rng.Double(0.2f, 0.1f);
		}

		[Test]
		public void TestFloatMinMax() {
			Statistics.Frequencies(new[] {1.0f}, () => (int) (Rng.Double(1.5f, 1.5f) - 1.5f));
			Statistics.Frequencies(new[] {1.0f}, () => (int) (Rng.Double(3.2f, 4.2f) - 3.2f));
			Statistics.Frequencies(new[] {0.5f, 0.5f}, () => (int) (Rng.Double(-4.5f, -2.5f) + 4.5f));
			Statistics.Frequencies(new[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => (int) ((Rng.Double(0.4f, 0.9f) - 0.4f) * 10.0f));
		}

		#endregion

		#region Point

		[Test, Repeat(10)]
		public void TestPointSize() {
			var size = new Size(5, 5);
			var point = Rng.Point(size);
			Assert.That(point.X >= 0 && point.Y >= 0 && point.X < size.Width && point.Y < size.Height, Is.True);
		}

		[Test, Repeat(10)]
		public void TestPointRect() {
			var rect = new Rectangle(3, 3, 7, 8);
			var point = Rng.Point(rect);

			Assert.That(rect.Contains(point), Is.True);
		}

		[Test, Repeat(10)]
		public void TestPointInclusiveRect() {
			var rect = new Rectangle(3, 3, 7, 8);
			var point = Rng.PointInclusive(rect);

			Assert.That(point.X >= rect.TopLeft.X && point.Y >= rect.TopLeft.Y && point.X <= rect.BottomRight.X && point.Y <= rect.BottomRight.Y, Is.True);
		}

		#endregion

		#region Item

		[Test, Repeat(10)]
		public void TestItem() {
			var intList = new List<int>();

			for (int i = 0; i < 10; i++) {
				intList.Add(Rng.Int(Int32.MaxValue));
			}

			Assert.That(intList.Contains(Rng.Item(intList)), Is.True);
		}

		[Test]
		public void TestItemWeighted() {
			List<int> items = new List<int>();
			List<int> weights = new List<int>
			                    {
			                    		100,
			                    		40,
			                    		59,
			                    		938,
			                    		485,
			                    		0,
			                    		9,
			                    		100
			                    };

			for (int i = 0; i < weights.Count; i++) {
				items.Add(i);
			}

			var sum = weights.Sum(i => i);
			var floats = weights.Select(i => ((float) i) / sum);

			Statistics.Frequencies(floats.ToArray(), () => Rng.ItemWeighted(items, i => weights[i]));
		}

		#endregion

		#region OneIn

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestOneInThrowsIfNegative() {
			Rng.OneIn(-2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestOneInThrowsIfZero() {
			Rng.OneIn(0);
		}

		[TestCase(1, 1.0f)]
		[TestCase(2, 0.5f)]
		[TestCase(3, 1.0f / 3.0f)]
		[TestCase(10, 0.1f)]
		public void TestOneIn(int max, float expected) {
			Statistics.Frequency(expected, () => Rng.OneIn(max));
		}

		#endregion

		#region Roll

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestRollThrowsIfDiceIsZero() {
			Rng.Roll(0, 6);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestRollThrowsIfDiceIsNegative() {
			Rng.Roll(-3, 6);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestRollThrowsIfSidesIsZero() {
			Rng.Roll(1, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestRollThrowsIfSidesIsNegative() {
			Rng.Roll(1, -3);
		}

		[Test]
		public void TestRoll() {
			// 5d1
			Statistics.Frequencies(
					new[] {0, 0, 0, 0, 0, 1.0f},
					() => Rng.Roll(5, 1));

			// 1d4
			Statistics.Frequencies(
					new[] {0, 0.25f, 0.25f, 0.25f, 0.25f},
					() => Rng.Roll(1, 4));

			// 2d6
			Statistics.Frequencies(
					new[]
					{
							0,
							0,
							1 / 36.0f, // 1+1
							2 / 36.0f, // 1+2 2+1
							3 / 36.0f, // 1+3 2+2 3+1
							4 / 36.0f, // 1+4 2+3 3+2 4+1
							5 / 36.0f, // 1+5 2+4 3+3 4+2 5+1
							6 / 36.0f, // 1+6 2+5 3+4 4+3 5+2 6+1
							5 / 36.0f, // 2+6 3+5 4+4 5+3 6+2
							4 / 36.0f, // 3+6 4+5 5+4 6+3
							3 / 36.0f, // 4+6 5+5 6+4
							2 / 36.0f, // 5+6 6+5
							1 / 36.0f, // 6+6
					},
					() => Rng.Roll(2, 6));
		}

		#endregion

		#region TriangleInt

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTriangleIntThrowsIfRangeIsNegative() {
			Rng.TriangleInt(5, -1);
		}

		[Test]
		public void TestTriangleInt() {
			// 3t0
			Console.WriteLine("3t0");
			Statistics.Frequencies(
					new[] {0, 0, 0, 1.0f, 0, 0},
					() => Rng.TriangleInt(3, 0));

			// 2t1
			Console.WriteLine("2t1");
			Statistics.Frequencies(
					new[] {0, 0.25f, 0.5f, 0.25f, 0},
					() => Rng.TriangleInt(2, 1));

			// 2t4 (+3)
			Console.WriteLine("2t4 + 3");
			Statistics.Frequencies(
					new[]
					{
							0,
							1 / 25.0f,
							2 / 25.0f,
							3 / 25.0f,
							4 / 25.0f,
							5 / 25.0f,
							4 / 25.0f,
							3 / 25.0f,
							2 / 25.0f,
							1 / 25.0f
					},
					() => Rng.TriangleInt(2, 4) + 3);
		}

		#endregion

		#region GaussianInt

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestGaussianIntThrowsOnNegativeRange() {
			Rng.GaussianInt(5, -1, 3);
		}

		[Test]
		public void TestGaussianInt() {
			Statistics.Frequencies(
					new[]
					{
							// 0
							(float) GaussianDistribution.At(0, 5, 3) + (float) GaussianDistribution.At(-1, 5, 3) + (float) GaussianDistribution.At(-2, 5, 3),
							// 1
							(float) GaussianDistribution.At(1, 5, 3),
							// 2
							(float) GaussianDistribution.At(2, 5, 3),
							// 3
							(float) GaussianDistribution.At(3, 5, 3),
							// 4
							(float) GaussianDistribution.At(4, 5, 3),
							// 5
							(float) GaussianDistribution.At(5, 5, 3),
							// 6
							(float) GaussianDistribution.At(6, 5, 3),
							// 7
							(float) GaussianDistribution.At(7, 5, 3),
							// 8
							(float) GaussianDistribution.At(8, 5, 3),
							// 9
							(float) GaussianDistribution.At(9, 5, 3),
							// 10
							(float) GaussianDistribution.At(10, 5, 3) + (float) GaussianDistribution.At(11, 5, 3) + (float) GaussianDistribution.At(12, 5, 3)
					},
					() => Rng.GaussianInt(5, 5, 3));
			Statistics.Frequencies(
					new[]
					{
							0,
							(float) GaussianDistribution.At(1, 10, 3),
							(float) GaussianDistribution.At(2, 10, 3),
							(float) GaussianDistribution.At(3, 10, 3),
							(float) GaussianDistribution.At(4, 10, 3),
							(float) GaussianDistribution.At(5, 10, 3),
							(float) GaussianDistribution.At(6, 10, 3),
							(float) GaussianDistribution.At(7, 10, 3),
							(float) GaussianDistribution.At(8, 10, 3),
							(float) GaussianDistribution.At(9, 10, 3),
							(float) GaussianDistribution.At(10, 10, 3),
							(float) GaussianDistribution.At(11, 10, 3),
							(float) GaussianDistribution.At(12, 10, 3),
							(float) GaussianDistribution.At(13, 10, 3),
							(float) GaussianDistribution.At(14, 10, 3),
							(float) GaussianDistribution.At(15, 10, 3),
							(float) GaussianDistribution.At(16, 10, 3),
							(float) GaussianDistribution.At(17, 10, 3),
							(float) GaussianDistribution.At(18, 10, 3),
							(float) GaussianDistribution.At(19, 10, 3)
					},
					() => Rng.GaussianInt(10, 9, 3));
		}

		#endregion

		#region Walk

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestWalkThrowsIfDecIsOne() {
			Rng.Walk(1, 1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestWalkThrowsIfIncIsOne() {
			Rng.Walk(1, 2, 1);
		}

		[Test]
		public void TestWalk() {
			// walk neither
			Statistics.Frequencies(
					new[] {0, 0, 1.0f, 0, 0},
					() => Rng.Walk(2, 0, 0));

			// walk up
			Statistics.Frequencies(
					new[] {1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 8.0f, 1.0f / 16.0f},
					() => Rng.Walk(0, 0, 2));

			// walk down
			Statistics.Frequencies(
					new[] {3.0f / 64.0f, 3.0f / 16.0f, 3.0f / 4.0f},
					() => Rng.Walk(2, 4, 0));

			// walk both
			Statistics.Frequencies(
					new[] {1 / 32.0f, 1 / 16.0f, 1 / 8.0f, 1 / 2.0f, 1 / 8.0f, 1 / 16.0f, 1 / 32.0f},
					() => Rng.Walk(3, 2, 2));
		}

		#endregion

		#region Taper

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfIncrementIsZero() {
			Rng.Taper(5, 0, 1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfChanceIsNegative() {
			Rng.Taper(5, 1, -2, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfChanceIsZero() {
			Rng.Taper(5, 1, 0, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfChanceIsRange() {
			Rng.Taper(5, 1, 2, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfChanceIsGreaterThanRange() {
			Rng.Taper(5, 1, 3, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfOutOfIsNegative() {
			Rng.Taper(5, 1, 1, -3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestTaperThrowsIfOutOfIsZero() {
			Rng.Taper(5, 1, 1, 0);
		}

		[Test]
		public void TestTaper() {
			// start at 3, increment by 1 50% of the time
			Statistics.Frequencies(
					new[] {0, 0, 0, 0.5f, 0.25f, 0.125f, 0.0625f},
					() => Rng.Taper(3, 1, 1, 2));

			// start at 1, increment by 2 50% of the time
			Statistics.Frequencies(
					new[] {0, 0.5f, 0, 0.25f, 0, 0.125f, 0, 0.0625f},
					() => Rng.Taper(1, 2, 3, 6));

			// start at 3, decrement by 1 25% of the time
			Statistics.Frequencies(
					new[] {0.01171875f, 0.046875f, 0.1875f, 0.75f},
					() => Rng.Taper(3, -1, 1, 4));
		}

		#endregion

	}
}