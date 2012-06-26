using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DEngine.Core {
    /// <summary>
    /// The Random Number God.
    /// </summary>
    public static class Rng {
        /// <summary>
        /// Resets the seed used to generate the random numbers to a time-dependent one.
        /// </summary>
        public static void Seed() {
            sRandom = new Random();
        }

        /// <summary>
        /// Sets the seed used to generate the random numbers.
        /// </summary>
        /// <param name="seed">New seed.</param>
        public static void Seed(int seed) {
            sRandom = new Random(seed);
        }

        /// <summary>
        /// Gets a random int between 0 and max (half-inclusive).
        /// </summary>
        public static int Int(int max) {
            return sRandom.Next(max);
        }

        /// <summary>
        /// Gets a random int between min and max (half-inclusive).
        /// </summary>
        public static int Int(int min, int max) {
            return Int(max - min) + min;
        }

        /// <summary>
        /// Gets a random int between 0 and max (inclusive).
        /// </summary>
        public static int IntInclusive(int max) {
            return sRandom.Next(max + 1);
        }

        /// <summary>
        /// Gets a random int between min and max (inclusive).
        /// </summary>
        public static int IntInclusive(int min, int max) {
            return IntInclusive(max - min) + min;
        }

        /// <summary>
        /// Gets a random double between 0 and max.
        /// </summary>
        public static double Double(double max = 1.0) {
            if (max < 0.0f)
                throw new ArgumentOutOfRangeException("max", "The max must be zero or greater.");

            return sRandom.NextDouble() * max;
        }

        /// <summary>
        /// Gets a random double between min and max.
        /// </summary>
        public static double Double(double min, double max) {
            if (max < min)
                throw new ArgumentOutOfRangeException("max", "The max must be min or greater.");

            return Double(max - min) + min;
        }

        /// <summary>
        /// Gets a random Point within the given size (half-inclusive).
        /// </summary>
        public static Point Point(Point size) {
            return new Point(Int(size.X), Int(size.Y));
        }

        /// <summary>
        /// Gets a random Point within the given Rect (half-inclusive).
        /// </summary>
        public static Point Point(Rect rect) {
            return new Point(Int(rect.Left, rect.Right), Int(rect.Top, rect.Bottom));
        }

        /// <summary>
        /// Gets a random Point within the given Rect (inclusive).
        /// </summary>
        public static Point PointInclusive(Rect rect) {
            return new Point(IntInclusive(rect.Left, rect.Right), IntInclusive(rect.Top, rect.Bottom));
        }

        /// <summary>
        /// Gets a random item from the given list.
        /// </summary>
        public static T Item<T>(IList<T> items) {
            return items[Int(items.Count)];
        }

        /// <summary>
        /// Returns true if a random int chosen between 1 and chance was 1.
        /// </summary>
        public static bool OneIn(int chance) {
            if (chance <= 0)
                throw new ArgumentOutOfRangeException("chance", "The chance must be one or greater.");

            return Int(chance) == 0;
        }

        public static bool Chance(double chance) {
            if (chance <= 0.0)
                throw new ArgumentOutOfRangeException("chance", "Chance must be greater than 0.0");
            if (chance > 1.0)
                throw new ArgumentOutOfRangeException("chance", "Chance must be less than 1.0");

            return sRandom.NextDouble() <= chance;
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
            if (dice <= 0)
                throw new ArgumentOutOfRangeException("dice", "The argument \"dice\" must be greater than zero.");
            if (sides <= 0)
                throw new ArgumentOutOfRangeException("sides", "The argument \"sides\" must be greater than zero.");

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
            if (range < 0)
                throw new ArgumentOutOfRangeException("range", "The argument \"range\" must be zero or greater.");

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
        /// Randomly walks the given starting value repeatedly up and/or down
        /// with the given probabilities. Will only walk in one direction.
        /// </summary>
        /// <param name="start">Value to start at.</param>
        public static int Walk(int start, int chanceOfDec, int chanceOfInc) {
            // make sure we won't get stuck in an infinite loop
            if (chanceOfDec == 1)
                throw new ArgumentOutOfRangeException("chanceOfDec", "chanceOfDec must be zero or greater than one.");
            if (chanceOfInc == 1)
                throw new ArgumentOutOfRangeException("chanceOfInc", "chanceOfInc must be zero greater than one.");

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
        /// Randomly walks the given level using a... unique distribution. The
        /// goal is to return a value that approximates a bell curve centered
        /// on the start level whose wideness increases as the level increases.
        /// Thus, starting at a low start level will only walk a short distance,
        /// while starting at a higher level can wander a lot farther.
        /// </summary>
        /// <returns></returns>
        public static int WalkLevel(int level) {
            int result = level;

            // stack a few triangles to approximate a bell
            for (int i = 0; i < Math.Min(5, level); i++)
                    // the width of the triangle is based on the level
                result += Rng.TriangleInt(0, 1 + (level / 20));

            // also have an exponentially descreasing change of going out of depth
            while (Rng.OneIn(10))
                result += 1 + Rng.Int(2 + (level / 5));

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
            if (increment == 0)
                throw new ArgumentOutOfRangeException("increment", "The increment cannot be zero.");
            if (chance <= 0)
                throw new ArgumentOutOfRangeException("chance", "The chance must be greater than zero.");
            if (chance >= outOf)
                throw new ArgumentOutOfRangeException("chance", "The chance must be less than the range.");
            if (outOf <= 0)
                throw new ArgumentOutOfRangeException("outOf", "The range must be positive.");

            int value = start;

            while (Rng.Int(outOf) < chance)
                value += increment;

            return value;
        }

        private static Random sRandom = new Random();
    }

    /// <summary>
    /// A roller rolls random numbers with certain parameters. It encapsulates a call
    /// to one of the Rng functions, and the parameters passed in.
    /// </summary>
    [Serializable]
    public class Rand {
        /// <summary>
        /// Tries to create a new Roller by parsing the given string representation of it.
        /// Valid strings include "3", "2-5", "2d6", "6t3", and "2-5(1:4)".
        /// </summary>
        /// <param name="text">The string to parse.</param>
        /// <returns>The parsed Roller or <c>null</c> if unsuccessful.</returns>
        public static Rand Parse(string text) {
            // ignore whitespace
            text = text.Trim();

            // compile the regex if needed
            if (parser == null) {
                string pattern =
                        @"^((?<die>(?<dice>\d+)d(?<sides>\d+))|(?<tri>(?<center>\d+)t(?<range>\d+))|(?<range>(?<min>\d+)-(?<max>\d+))|(?<fixed>(?<value>-?\d+)))(?<taper>\((?<chance>\d+)\:(?<outof>\d+)\))?$";

                parser = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            }

            // parse it
            Match match = parser.Match(text);

            if (!match.Success)
                return null;

            Rand rand;

            if (match.Groups["die"].Success) {
                int dice = Int32.Parse(match.Groups["dice"].Value);
                int sides = Int32.Parse(match.Groups["sides"].Value);
                rand = Rand.Dice(dice, sides);
            } else if (match.Groups["tri"].Success) {
                int center = Int32.Parse(match.Groups["center"].Value);
                int range = Int32.Parse(match.Groups["range"].Value);
                rand = Rand.Triangle(center, range);
            } else if (match.Groups["range"].Success) {
                int min = Int32.Parse(match.Groups["min"].Value);
                int max = Int32.Parse(match.Groups["max"].Value);
                rand = Rand.Range(min, max);
            } else // fixed
            {
                int value = Int32.Parse(match.Groups["value"].Value);
                rand = Rand.Constant(value);
            }

            // add the taper
            if (match.Groups["taper"].Success) {
                int chance = Int32.Parse(match.Groups["chance"].Value);
                int outOf = Int32.Parse(match.Groups["outof"].Value);

                rand.nextRand = Taper(chance, outOf);
            }

            return rand;
        }

        public static Rand Constant(int value) {
            return new Rand(
                    () => value,
                    value, value.ToString(), true);
        }

        public static Rand Range(int min, int max) {
            return new Rand(
                    () => Rng.IntInclusive(min, max),
                    (min + max) / 2.0f, min.ToString() + "-" + max.ToString());
        }

        public static Rand Dice(int dice, int sides) {
            return new Rand(
                    () => Rng.Roll(dice, sides),
                    dice * ((1 + sides) / 2.0f), dice.ToString() + "d" + sides.ToString());
        }

        public static Rand Triangle(int center, int range) {
            return new Rand(
                    () => Rng.TriangleInt(center, range),
                    center, center.ToString() + "t" + range.ToString());
        }

        public static Rand Taper(int chance, int outOf) {
            return new Rand(
                    () => Rng.Taper(0, 1, chance, outOf),
                    (float) chance / (outOf - (float) chance), // sum of geometric series
                    "(" + chance + ":" + outOf + ")");
        }

        /// <summary>
        /// Gets whether this Roller always returns a fixed value.
        /// </summary>
        public bool IsConstant {
            get {
                bool isConstant = this.constant;
                if (nextRand != null)
                    isConstant &= nextRand.IsConstant;

                return isConstant;
            }
        }

        /// <summary>
        /// Gets the average of the values rolled by this Roller.
        /// </summary>
        public float Average {
            get {
                float avrg = this.average;
                if (nextRand != null)
                    avrg += nextRand.Average;

                return avrg;
            }
        }

        public override string ToString() {
            string text = this.text;
            if (nextRand != null)
                text += nextRand.ToString();

            return text;
        }

        public int Roll() {
            int result = rollFunction();
            if (nextRand != null)
                result += nextRand.Roll();

            return result;
        }

        private Rand(Func<int> rollFunction, float average, string text, bool isConstant) {
            this.rollFunction = rollFunction;
            this.average = average;
            this.text = text;
            this.constant = isConstant;
        }

        private Rand(Func<int> rollFunction, float average, string text)
                : this(rollFunction, average, text, false) {}

        private static Regex parser;

        private readonly Func<int> rollFunction;
        private readonly float average;
        private readonly string text;
        private readonly bool constant;

        // allows rollers to be chained: 2d6 + 3d4 + 1t4...
        private Rand nextRand;

        public static Rand operator +(Rand v1, Rand v2) {
            return new Rand(v2.rollFunction, v2.Average, v2.text, v2.IsConstant) {nextRand = v1};            
        }
    }
}