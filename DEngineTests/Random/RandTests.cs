using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using DEngine.Random;
using DEngineTests.Util;
using NUnit.Framework;

namespace DEngineTests.Random {
	[TestFixture]
	public class RandTests {
		#region Parse

		[Test]
		public static void TestParseFixed() {
			for (int outOf = 2; outOf < 10; outOf++) {
				for (int chance = 1; chance < outOf; chance++) {
					float ave = Rand.Taper(chance, outOf).Average;
					Console.WriteLine("taper " + chance + "/" + outOf + " = " + ave);
				}
			}

			Parse("3", "3", 3,		new[] { 0, 0, 0, 1.0f, 0 });
			Parse(" 2", "2", 2,		new[] { 0, 0, 1.0f, 0 });
			Parse(" 1  ", "1", 1,	new[] { 0, 1.0f, 0 });
		}

		[Test]
		public static void TestParseRange() {
			Parse("2-6", "2-6", 4,		new[] { 0, 0, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0 });
			Parse(" 1-2", "1-2", 1.5f,	new[] { 0, 0.5f, 0.5f, 0 });
			Parse(" 3-3  ", "3-3", 3,	new[] { 0, 0, 0, 1.0f, 0 });
		}

		[Test]
		public static void TestParseDice() {
			Parse(" 1d5", "1d5", 3,		new[] { 0, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0 });

			Parse(" 2d3  ", "2d3", 4, new[]
			                          {
			                          		0,
			                          		0,
			                          		1 / 9.0f,
			                          		2 / 9.0f,
			                          		3 / 9.0f,
			                          		2 / 9.0f,
			                          		1 / 9.0f,
			                          		0
			                          });
		}

		[Test]
		public static void TestParseTriangle() {
			Parse(" 2t1  ", "2t1", 2, new[] { 0, 0.25f, 0.5f, 0.25f, 0 });

			Parse("2t4", "2t4", 2, new[]
			                       {
			                       		3 / 25.0f,
			                       		4 / 25.0f,
			                       		5 / 25.0f,
			                       		4 / 25.0f,
			                       		3 / 25.0f,
			                       		2 / 25.0f,
			                       		1 / 25.0f
			                       });

			Parse(" 3t0", "3t0", 3, new[] { 0, 0, 0, 1.0f, 0, 0 });
		}

		[Test]
		public static void TestParseFixedTaper() {
			// geometric series sums
			float taperAverageFour = 1.0f / 3.0f; // 1 in 4
			float taperAverageTwo = 1.0f; // 1 in 2

			Parse("3+(1:4)", "3 + (1:4)", 3 + taperAverageFour);
			Parse(" 2+(1:2)", "2 + (1:2)", 2 + taperAverageTwo);
			Parse("1+(3:4)", "1 + (3:4)", 1 + (3.0f / (4.0f - 3.0f)));
		}

		[Test]
		public static void TestParseRangeTaper() {
			// geometric series sums
			float taperAverageFour = 1.0f / 3.0f; // 1 in 4
			float taperAverageTwo = 1.0f; // 1 in 2

			Parse("2-6+(1:4)", "2-6 + (1:4)", 4 + taperAverageFour);
			Parse(" 1-2+(1:2)", "1-2 + (1:2)", 1.5f + taperAverageTwo);
		}

		[Test]
		public static void TestParseDiceTaper() {
			// geometric series sums
			float taperAverageFour = 1.0f / 3.0f; // 1 in 4
			float taperAverageTwo = 1.0f; // 1 in 2

			Parse("1d5 + (1:4)", "1d5 + (1:4)", 3 + taperAverageFour);
			Parse(" 2d3+(1:2)", "2d3 + (1:2)", 4 + taperAverageTwo);
		}

		[Test]
		public static void TestParseTriangleTaper() {
			// geometric series sums
			float taperAverageFour = 1.0f / 3.0f; // 1 in 4
			float taperAverageTwo = 1.0f; // 1 in 2

			Parse("2t1+(1:4)", "2t1 + (1:4)", 2 + taperAverageFour);
			Parse(" 2t4+(1:2)", "2t4 + (1:2)", 2 + taperAverageTwo);
		}

		[Test]
		public static void TestChaining() {
			var r0 = Rand.Constant(-3);
			var r1 = Rand.Dice(2, 4);
			var r2 = Rand.Range(2, 10);
			var r3 = Rand.Taper(1, 6);
			var r4 = Rand.Triangle(10, 7);

			var chain = r0 + r1 + r2 + r3 + r4;
			Parse("10t7 + (1:6) + 2-10 + 2d4 + -3", chain.ToString(), r0.Average + r1.Average + r2.Average + r3.Average + r4.Average);
		}

		#endregion

		#region Helper methods

		private static void Parse(string text, string expected, float average, float[] frequencies = null) {
			Rand rand = Rand.Parse(text);

			Assert.AreEqual(expected, rand.ToString());
			Assert.AreEqual(average, rand.Average);

			if (frequencies != null) {
				Statistics.Frequencies(frequencies, rand.Roll);
			}
		}

		#endregion
	}
}
