using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DEngine.Core;

namespace DEngine.Random {
	/// <summary>
	/// The Random Number God.
	/// </summary>
	public static class Rng {
		/// <summary>
		/// Resets the seed used to generate the random numbers to a time-dependent one.
		/// </summary>
		public static void Seed() {
			_random = new System.Random();
		}

		/// <summary>
		/// Sets the seed used to generate the random numbers.
		/// </summary>
		/// <param name="seed">New seed.</param>
		public static void Seed(int seed) {
			_random = new System.Random(seed);
		}

		/// <summary>
		/// Gets a random int between 0 (inclusive) and max (exclusive).
		/// </summary>
		public static int Int(int max) {
			Contract.Requires<ArgumentOutOfRangeException>(max > 0);
			Contract.Ensures(Contract.Result<int>() >= 0 && Contract.Result<int>() < max);
			return _random.Next(max);
		}

		/// <summary>
		/// Gets a random int between min (inclusive) and max (exclusive).
		/// </summary>
		public static int Int(int min, int max) {
			Contract.Requires<ArgumentOutOfRangeException>(min < max);
			Contract.Ensures(Contract.Result<int>() >= min && Contract.Result<int>() < max);

			return Int(max - min) + min;
		}

		/// <summary>
		/// Gets a random int between 0 and max (inclusive).
		/// </summary>
		public static int IntInclusive(int max) {
			Contract.Requires<ArgumentOutOfRangeException>(max >= 0);
			Contract.Ensures(Contract.Result<int>() >= 0 && Contract.Result<int>() <= max);
			return _random.Next(max + 1);
		}

		/// <summary>
		/// Gets a random int between min and max (inclusive).
		/// </summary>
		public static int IntInclusive(int min, int max) {
			Contract.Requires<ArgumentOutOfRangeException>(min <= max);
			Contract.Ensures(Contract.Result<int>() >= min && Contract.Result<int>() <= max);
			return IntInclusive(max - min) + min;
		}

		/// <summary>
		/// Gets a random double between 0 and max.
		/// </summary>
		public static double Double(double max = 1.0) {
			Contract.Requires<ArgumentOutOfRangeException>(max >= 0.0f, "The max must be zero or greater.");
			
			return _random.NextDouble() * max;
		}

		/// <summary>
		/// Gets a random double between min and max.
		/// </summary>
		public static double Double(double min, double max) {
			Contract.Requires<ArgumentOutOfRangeException>(max >= min, "The max must be min or greater.");

			return Double(max - min) + min;
		}

		/// <summary>
		/// Gets a random Point within the given size (half-inclusive).
		/// </summary>
		public static Point Point(Size size) {
			Contract.Requires<ArgumentException>(size.Width > 0);
			Contract.Requires<ArgumentException>(size.Height > 0);

			return new Point(Int(size.Width), Int(size.Height));
		}

		/// <summary>
		/// Gets a random Point within the given Rect (half-inclusive).
		/// </summary>
		public static Point Point(Rectangle rect) {
			return new Point(Int(rect.Left, rect.Right - 1), Int(rect.Top, rect.Bottom - 1));
		}

		/// <summary>
		/// Gets a random Point within the given Rect (inclusive).
		/// </summary>
		public static Point PointInclusive(Rectangle rect) {
			return new Point(IntInclusive(rect.Left, rect.Right - 1), IntInclusive(rect.Top, rect.Bottom - 1));
		}

		/// <summary>
		/// Gets a random item from the given list.
		/// </summary>
		public static T Item<T>(IList<T> items) {
			Contract.Requires<ArgumentNullException>(items != null, "items");
			Contract.Requires<ArgumentException>(items.Count > 0);
			return items[Int(items.Count)];
		}

		/// <summary>
		/// Gets a random item from the given list.
		/// </summary>
		public static T ItemWeighted<T>(IList<T> items, Func<T, int> weightFunction) {
			Contract.Requires<ArgumentNullException>(items != null, "items");
			Contract.Requires<ArgumentException>(items.Count > 0);

			var r = Rng.Int(items.Sum(weightFunction));

			int total = 0;
			foreach (var i in items) {
				total += weightFunction(i);

				if (r <= total)
					return i;
			}

			throw new Exception("How did we get here?  We have somehow given a random int that is more than the total sums of the weights");
		}

		/// <summary>
		/// Returns true if a random int chosen between 1 and chance was 1.
		/// </summary>
		public static bool OneIn(int chance) {
			Contract.Requires<ArgumentOutOfRangeException>(chance > 0, "The chance must be one or greater.");

			return Int(chance) == 0;
		}

		/// <summary>
		/// Returns true if given a random double (between 0.0 and 1.0) will be less or equal to than the given argument.
		/// </summary>
		/// <param name="chance"></param>
		/// <returns></returns>
		public static bool Chance(double chance) {
			Contract.Requires<ArgumentOutOfRangeException>(chance > 0.0, "Chance must be greater than 0.0");
			Contract.Requires<ArgumentOutOfRangeException>(chance < 1.0, "Chance must be less than 1.0");

			return _random.NextDouble() <= chance;
		}

		/// <summary>
		/// Rolls the given number of dice with the given number of sides on each die,
		/// and returns the sum. The values on each side range from 1 to the number of
		/// sides.
		/// </summary>
		/// <param name="dice">Number of dice to roll.</param>
		/// <param name="sides">Number of sides on each dice.</param>
		/// <returns>The sum of the dice rolled.</returns>
		public static int Roll(int dice, int sides) {
			Contract.Requires<ArgumentOutOfRangeException>(dice > 0, "The argument \"dice\" must be greater than zero.");
			Contract.Requires<ArgumentOutOfRangeException>(sides > 0, "The argument \"sides\" must be greater than zero.");

			int total = 0;

			for (int i = 0; i < dice; i++)
				total += Int(1, sides + 1);

			return total;
		}

		/// <summary>
		/// Gets a random number centered around "center" with range "range" (inclusive)
		/// using a triangular distribution. For example getTriInt(8, 4) will return values
		/// between 4 and 12 (inclusive) with greater distribution towards 8.
		/// </summary>
		/// <remarks>
		/// This means output values will range from (center - range) to (center + range)
		/// inclusive, with most values near the center, but not with a normal distribution.
		/// Think of it as a poor man's bell curve.
		///
		/// The algorithm works essentially by choosing a random point inside the triangle,
		/// and then calculating the x coordinate of that point. It works like this:
		///
		/// Consider Center 4, Range 3:
		/// 
		///         *
		///       * | *
		///     * | | | *
		///   * | | | | | *
		/// --+-----+-----+--
		/// 0 1 2 3 4 5 6 7 8
		///  -r     c     r
		/// 
		/// Now flip the left half of the triangle (from 1 to 3) vertically and move it
		/// over to the right so that we have a square.
		/// 
		///     +-------+
		///     |       V
		///     |
		///     |   R L L L
		///     | . R R L L
		///     . . R R R L
		///   . . . R R R R
		/// --+-----+-----+--
		/// 0 1 2 3 4 5 6 7 8
		/// 
		/// Choose a point in that square. Figure out which half of the triangle the
		/// point is in, and then remap the point back out to the original triangle.
		/// The result is the x coordinate of the point in the original triangle.
		/// </remarks>
		public static int TriangleInt(int center, int range) {
			Contract.Requires<ArgumentOutOfRangeException>(range >= 0, "The argument \"range\" must be zero or greater.");

			// pick a point in the square
			int x = IntInclusive(range);
			int y = IntInclusive(range);

			// figure out which triangle we are in
			if (x <= y)
					// larger triangle
				return center + x;
			else
					// smaller triangle
				return center - range - 1 + x;
		}

		/// <summary>
		/// Gets a random number centered around "center" with range "range" (inclusive)
		/// using a gaussian/standard distribution. For example GaussianInt(8, 4, 3) will return values
		/// between 4 and 12 (inclusive) with greater distribution towards 8.  Standard deviation will 
		/// cause the distribution to vary its weights
		/// </summary>
		/// <param name="center">mean</param>
		/// <param name="range">inclusive range, <b>this can alter the distribution curve greatly is range is too small compared to std dev</b></param>
		/// <param name="stddev">standard deviation</param>
		/// <returns></returns>        
		public static int GaussianInt(int center, int range, int stddev) {
			Contract.Requires<ArgumentOutOfRangeException>(range >= 0, "The argument \"range\" must be zero or greater.");

			return (int) Math.Round(GaussianDouble(center, range, stddev));
		}

		public static double GaussianDouble(double center, double range, double stddev) {
			Contract.Requires<ArgumentOutOfRangeException>(range >= 0, "The argument \"range\" must be zero or greater.");

			double gaussianDouble = GaussianDistribution.InverseCumulativeTo(Double(), center, stddev);

			if (gaussianDouble < center - range)
				return center - range;
			else if (gaussianDouble > center + range)
				return center + range;
			else
				return gaussianDouble;
		}

		/// <summary>
		/// Randomly walks the given starting value repeatedly up and/or down
		/// with the given probabilities. Will only walk in one direction.
		/// </summary>
		/// <param name="start">Value to start at.</param>
		/// <param name = "chanceOfDec"></param>
		/// <param name = "chanceOfInc"></param>
		public static int Walk(int start, int chanceOfDec, int chanceOfInc) {
			// make sure we won't get stuck in an infinite loop
			Contract.Requires<ArgumentOutOfRangeException>(chanceOfDec != 1, "chanceOfDec must be zero or greater than one.");
			Contract.Requires<ArgumentOutOfRangeException>(chanceOfInc != 1, "chanceOfInc must be zero greater than one.");

			if (chanceOfDec == 0 && chanceOfInc == 0)
				return start;

			// decide if walking up or down
			int direction = Int(chanceOfDec + chanceOfInc);
			if (direction < chanceOfDec) {
				// exponential chance of decrementing
				int sanity = 10000;
				while (Rng.OneIn(chanceOfDec) && (sanity-- > 0))
					start--;
			} else if (direction < chanceOfDec + chanceOfInc) {
				// exponential chance of incrementing
				int sanity = 10000;
				while (Rng.OneIn(chanceOfInc) && (sanity-- > 0))
					start++;
			}

			return start;
		}

		/// <summary>
		/// Randomly walks the given level using a... unique distribution (triangle). The
		/// goal is to return a value that approximates a bell curve centered
		/// on the start level whose wideness increases as the level increases.
		/// Thus, starting at a low start level will only walk a short distance,
		/// while starting at a higher level can wander a lot farther.
		/// </summary>
		/// <returns></returns>
		public static int WalkLevelTriangle(int level) {
			int result = level;

			// stack a few triangles to approximate a bell
			for (int i = 0; i < Math.Min(5, level); i++) {
				// the width of the triangle is based on the level
				result += Rng.TriangleInt(0, 1 + (level / 20));
			}

			// also have an exponentially descreasing change of going out of depth
			while (Rng.OneIn(10)) {
				result += 1 + Rng.Int(2 + (level / 5));
			}

			return result;
		}

		/// <summary>
		/// Repeatedly adds an increment to a given starting value as long as random
		/// numbers continue to be chosen from within a given range. Yields numbers
		/// whose probability gradually tapers off from the starting value.
		/// </summary>
		/// <param name="start">Starting value.</param>
		/// <param name="increment">Amount to modify starting value every successful
		/// iteration.</param>
		/// <param name="chance">The odds of an iteration being successful.</param>
		/// <param name="outOf">The range to choose from to see if the iteration
		/// is successful.</param>
		/// <returns>The resulting value.</returns>
		public static int Taper(int start, int increment, int chance, int outOf) {
			Contract.Requires<ArgumentOutOfRangeException>(increment != 0, "The increment cannot be zero.");
			Contract.Requires<ArgumentOutOfRangeException>(chance > 0, "The chance must be greater than zero.");
			Contract.Requires<ArgumentOutOfRangeException>(chance < outOf, "The chance must be less than the range.");
			Contract.Requires<ArgumentOutOfRangeException>(outOf > 0, "The range must be positive.");

			int value = start;

			while (Rng.Int(outOf) < chance)
				value += increment;

			return value;
		}

		private static System.Random _random = new System.Random();
	}
}