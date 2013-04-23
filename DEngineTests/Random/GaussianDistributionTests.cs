using System;
using DEngine.Core;
using DEngine.Random;
using NUnit.Framework;

namespace DEngineTests.Random {
	[TestFixture]
	public class GaussianDistributionTests {
		private const double ERROR_TOLERANCE = 0.000001;

		[Test]
		public static void TestCumulativeTo() {
			// Verified with WolframAlpha
			// (e.g. http://www.wolframalpha.com/input/?i=CDF%5BNormalDistribution%5B0%2C1%5D%2C+0.5%5D )
			Assert.AreEqual(0.691462, GaussianDistribution.CumulativeTo(0.5), ERROR_TOLERANCE);
		}

		[Test]
		public static void TestAt() {
			// Verified with WolframAlpha
			// (e.g. http://www.wolframalpha.com/input/?i=PDF%5BNormalDistribution%5B0%2C1%5D%2C+0.5%5D )
			Assert.AreEqual(0.352065, GaussianDistribution.At(0.5), ERROR_TOLERANCE);
		}

		[Test]
		public static void TestMultiplication() {
			// I verified this against the formula at http://www.tina-vision.net/tina-knoppix/tina-memo/2003-003.pdf
			var standardNormal = new GaussianDistribution(0, 1);
			var shiftedGaussian = new GaussianDistribution(2, 3);

			var product = standardNormal * shiftedGaussian;

			Assert.AreEqual(0.2, product.Mean, ERROR_TOLERANCE);
			Assert.AreEqual(3.0 / Math.Sqrt(10), product.StandardDeviation, ERROR_TOLERANCE);

			var m4s5 = new GaussianDistribution(4, 5);
			var m6s7 = new GaussianDistribution(6, 7);

			var product2 = m4s5 * m6s7;
			Func<double, double> square = x => x * x;

			var expectedMean = (4 * square(7) + 6 * square(5)) / (square(5) + square(7));
			Assert.AreEqual(expectedMean, product2.Mean, ERROR_TOLERANCE);

			var expectedSigma = Math.Sqrt(((square(5) * square(7)) / (square(5) + square(7))));
			Assert.AreEqual(expectedSigma, product2.StandardDeviation, ERROR_TOLERANCE);
		}

		[Test]
		public static void TestDivision() {
			// Since the multiplication was worked out by hand, we use the same numbers but work backwards
			var product = new GaussianDistribution(0.2, 3.0 / Math.Sqrt(10));
			var standardNormal = new GaussianDistribution(0, 1);

			var productDividedByStandardNormal = product / standardNormal;
			Assert.AreEqual(2.0, productDividedByStandardNormal.Mean, ERROR_TOLERANCE);
			Assert.AreEqual(3.0, productDividedByStandardNormal.StandardDeviation, ERROR_TOLERANCE);

			Func<double, double> square = x => x * x;
			var product2 = new GaussianDistribution((4 * square(7) + 6 * square(5)) / (square(5) + square(7)), Math.Sqrt(((square(5) * square(7)) / (square(5) + square(7)))));
			var m4s5 = new GaussianDistribution(4, 5);
			var product2DividedByM4S5 = product2 / m4s5;
			Assert.AreEqual(6.0, product2DividedByM4S5.Mean, ERROR_TOLERANCE);
			Assert.AreEqual(7.0, product2DividedByM4S5.StandardDeviation, ERROR_TOLERANCE);
		}

		[Test]
		public static void TestLogProductNormalization() {
			// Verified with Ralf Herbrich's F# implementation
			var standardNormal = new GaussianDistribution(0, 1);
			var lpn = GaussianDistribution.LogProductNormalization(standardNormal, standardNormal);
			Assert.AreEqual(-1.2655121234846454, lpn, ERROR_TOLERANCE);

			var m1s2 = new GaussianDistribution(1, 2);
			var m3s4 = new GaussianDistribution(3, 4);
			var lpn2 = GaussianDistribution.LogProductNormalization(m1s2, m3s4);
			Assert.AreEqual(-2.5168046699816684, lpn2, ERROR_TOLERANCE);
		}

		[Test]
		public static void TestLogRatioNormalization() {
			// Verified with Ralf Herbrich's F# implementation            
			var m1s2 = new GaussianDistribution(1, 2);
			var m3s4 = new GaussianDistribution(3, 4);
			var lrn = GaussianDistribution.LogRatioNormalization(m1s2, m3s4);
			Assert.AreEqual(2.6157405972171204, lrn, ERROR_TOLERANCE);
		}

		[Test]
		public static void TestAbsoluteDifference() {
			// Verified with Ralf Herbrich's F# implementation            
			var standardNormal = new GaussianDistribution(0, 1);
			var absDiff = GaussianDistribution.AbsoluteDifference(standardNormal, standardNormal);
			Assert.AreEqual(0.0, absDiff, ERROR_TOLERANCE);

			var m1s2 = new GaussianDistribution(1, 2);
			var m3s4 = new GaussianDistribution(3, 4);
			var absDiff2 = GaussianDistribution.AbsoluteDifference(m1s2, m3s4);
			Assert.AreEqual(0.4330127018922193, absDiff2, ERROR_TOLERANCE);
		}
	}
}