using System;
using System.Collections.Generic;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests.Random {
	[TestFixture]
	internal class RngTests {
		#region Seed

		[Test]
		public void Seed() {
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
		public void IntMaxThrowsIfNegative() {
			Rng.Int(-2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IntMaxThrowsIfZero() {
			Rng.Int(0);
		}

		[Test]
		public void IntMax() {
			//			Statistics.TestFrequencies(new float[] { 1.0f }, () => Rng.Int(0));
			Statistics.Frequencies(new float[] {1.0f}, () => Rng.Int(1));
			Statistics.Frequencies(new float[] {0.5f, 0.5f}, () => Rng.Int(2));
			Statistics.Frequencies(new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => Rng.Int(5));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IntMinMaxThrowsBadRange() {
			Rng.Int(5, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IntMinMaxThrowsIfMaxEqualsMin() {
			Rng.Int(5, 5);
		}

		[Test]
		public void IntMinMax() {
			//			Statistics.TestFrequencies(new float[] { 1.0f }, () => Rng.Int(5, 5) - 5);
			Statistics.Frequencies(new float[] {1.0f}, () => Rng.Int(3, 4) - 3);
			Statistics.Frequencies(new float[] {0.5f, 0.5f}, () => Rng.Int(-4, -2) + 4);
			Statistics.Frequencies(new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => Rng.Int(4, 9) - 4);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IntInclusiveMaxThrowsIfNegative() {
			Rng.IntInclusive(-2);
		}

		[Test]
		public void IntInclusiveMax() {
			Statistics.Frequencies(new float[] {1.0f}, () => Rng.IntInclusive(0));
			Statistics.Frequencies(new float[] {0.5f, 0.5f}, () => Rng.IntInclusive(1));
			Statistics.Frequencies(new float[] {0.25f, 0.25f, 0.25f, 0.25f}, () => Rng.IntInclusive(3));
			Statistics.Frequencies(new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => Rng.IntInclusive(4));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IntInclusiveMinMaxThrowsBadRange() {
			Rng.IntInclusive(5, 3);
		}

		[Test]
		public void IntInclusiveMinMax() {
			Statistics.Frequencies(new float[] {1.0f}, () => Rng.IntInclusive(5, 5) - 5);
			Statistics.Frequencies(new float[] {0.5f, 0.5f}, () => Rng.IntInclusive(3, 4) - 3);
			Statistics.Frequencies(new float[] {0.25f, 0.25f, 0.25f, 0.25f}, () => Rng.IntInclusive(-5, -2) + 5);
			Statistics.Frequencies(new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => Rng.IntInclusive(4, 8) - 4);
		}

		#endregion

		#region Float

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void FloatMaxThrowsIfNegative() {
			Rng.Double(-2);
		}

		[Test]
		public void FloatMax() {
			Statistics.Frequencies(new float[] {1.0f}, () => (int) Rng.Double(0));
			Statistics.Frequencies(new float[] {1.0f}, () => (int) Rng.Double(1));
			Statistics.Frequencies(new float[] {0.5f, 0.5f}, () => (int) Rng.Double(2));
			Statistics.Frequencies(new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => (int) (Rng.Double(0.5f) * 10.0f));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void FloatMinMaxThrowsBadRange() {
			Rng.Double(0.2f, 0.1f);
		}

		[Test]
		public void FloatMinMax() {
			Statistics.Frequencies(new float[] {1.0f}, () => (int) (Rng.Double(1.5f, 1.5f) - 1.5f));
			Statistics.Frequencies(new float[] {1.0f}, () => (int) (Rng.Double(3.2f, 4.2f) - 3.2f));
			Statistics.Frequencies(new float[] {0.5f, 0.5f}, () => (int) (Rng.Double(-4.5f, -2.5f) + 4.5f));
			Statistics.Frequencies(new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f}, () => (int) ((Rng.Double(0.4f, 0.9f) - 0.4f) * 10.0f));
		}

		#endregion

		#region Vec

		#endregion

		#region Item

		#endregion

		#region OneIn

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void OneInThrowsIfNegative() {
			Rng.OneIn(-2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void OneInThrowsIfZero() {
			Rng.OneIn(0);
		}

		[Test]
		public void OneIn() {
			OneIn(1, 1.0f);
			OneIn(2, 0.5f);
			OneIn(3, 1.0f / 3.0f);
			OneIn(10, 0.1f);
		}

		#endregion

		#region Roll

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RollThrowsIfDiceIsZero() {
			Rng.Roll(0, 6);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RollThrowsIfDiceIsNegative() {
			Rng.Roll(-3, 6);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RollThrowsIfSidesIsZero() {
			Rng.Roll(1, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RollThrowsIfSidesIsNegative() {
			Rng.Roll(1, -3);
		}

		[Test]
		public void Roll() {
			// 5d1
			Statistics.Frequencies(
					new float[] {0, 0, 0, 0, 0, 1.0f},
					() => Rng.Roll(5, 1));

			// 1d4
			Statistics.Frequencies(
					new float[] {0, 0.25f, 0.25f, 0.25f, 0.25f},
					() => Rng.Roll(1, 4));

			// 2d6
			Statistics.Frequencies(
					new float[]
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
		public void TriangleIntThrowsIfRangeIsNegative() {
			Rng.TriangleInt(5, -1);
		}

		[Test]
		public void TriangleInt() {
			// 3t0
			Console.WriteLine("3t0");
			Statistics.Frequencies(
					new float[] {0, 0, 0, 1.0f, 0, 0},
					() => Rng.TriangleInt(3, 0));

			// 2t1
			Console.WriteLine("2t1");
			Statistics.Frequencies(
					new float[] {0, 0.25f, 0.5f, 0.25f, 0},
					() => Rng.TriangleInt(2, 1));

			// 2t4 (+3)
			Console.WriteLine("2t4 + 3");
			Statistics.Frequencies(
					new float[]
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
							1 / 25.0f,
					},
					() => Rng.TriangleInt(2, 4) + 3);
		}

		#endregion

		#region GaussianInt

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void GaussianIntThrowsOnNegativeRange() {
			Rng.GaussianInt(5, -1, 3);
		}

		[Test]
		public void GaussianInt() {
			Statistics.Frequencies(
					new float[]
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
							(float) GaussianDistribution.At(10, 5, 3) + (float) GaussianDistribution.At(11, 5, 3) + (float) GaussianDistribution.At(12, 5, 3),
					},
					() => Rng.GaussianInt(5, 5, 3));
			Statistics.Frequencies(
					new float[]
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
							(float) GaussianDistribution.At(19, 10, 3),
					},
					() => Rng.GaussianInt(10, 9, 3));
		}

		#endregion

		#region Walk

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WalkThrowsIfDecIsOne() {
			Rng.Walk(1, 1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WalkThrowsIfIncIsOne() {
			Rng.Walk(1, 2, 1);
		}

		[Test]
		public void Walk() {
			// walk neither
			Statistics.Frequencies(
					new float[] {0, 0, 1.0f, 0, 0},
					() => Rng.Walk(2, 0, 0));

			// walk up
			Statistics.Frequencies(
					new float[] {1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 8.0f, 1.0f / 16.0f},
					() => Rng.Walk(0, 0, 2));

			// walk down
			Statistics.Frequencies(
					new float[] {3.0f / 64.0f, 3.0f / 16.0f, 3.0f / 4.0f},
					() => Rng.Walk(2, 4, 0));

			// walk both
			Statistics.Frequencies(
					new float[] {1 / 32.0f, 1 / 16.0f, 1 / 8.0f, 1 / 2.0f, 1 / 8.0f, 1 / 16.0f, 1 / 32.0f},
					() => Rng.Walk(3, 2, 2));
		}

		#endregion

		#region Taper

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfIncrementIsZero() {
			Rng.Taper(5, 0, 1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfChanceIsNegative() {
			Rng.Taper(5, 1, -2, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfChanceIsZero() {
			Rng.Taper(5, 1, 0, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfChanceIsRange() {
			Rng.Taper(5, 1, 2, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfChanceIsGreaterThanRange() {
			Rng.Taper(5, 1, 3, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfOutOfIsNegative() {
			Rng.Taper(5, 1, 1, -3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TaperThrowsIfOutOfIsZero() {
			Rng.Taper(5, 1, 1, 0);
		}

		[Test]
		public void Taper() {
			// start at 3, increment by 1 50% of the time
			Statistics.Frequencies(
					new float[] {0, 0, 0, 0.5f, 0.25f, 0.125f, 0.0625f},
					() => Rng.Taper(3, 1, 1, 2));

			// start at 1, increment by 2 50% of the time
			Statistics.Frequencies(
					new float[] {0, 0.5f, 0, 0.25f, 0, 0.125f, 0, 0.0625f},
					() => Rng.Taper(1, 2, 3, 6));

			// start at 3, decrement by 1 25% of the time
			Statistics.Frequencies(
					new float[] {0.01171875f, 0.046875f, 0.1875f, 0.75f},
					() => Rng.Taper(3, -1, 1, 4));
		}

		#endregion

		#region Helper methods

		private void OneIn(int max, float expected) {
			Statistics.Frequency(expected, () => Rng.OneIn(max));
		}

		#endregion

	}
}