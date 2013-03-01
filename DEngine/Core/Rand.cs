using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace DEngine.Core {
	/// <summary>
	/// Rolls random numbers with certain parameters. It encapsulates a call
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
//			text = text.Trim();

			// compile the regex if needed
			if (parser == null) {
				const string pattern = @"^((?<die>(?<dice>\d+)d(?<sides>\d+))|(?<tri>(?<center>\d+)t(?<range>\d+))|(?<range>(?<min>\d+)-(?<max>\d+))|(?<fixed>(?<value>-?\d+)))|(?<taper>\((?<chance>\d+)\:(?<outof>\d+)\))?$";

				parser = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
			}

			var rands = text.Split(new char[] {'+'}, StringSplitOptions.RemoveEmptyEntries);
			
			Rand rand = null;
			Rand prev = null;

			foreach (var s in rands) {
				Match m = parser.Match(s.Trim());

				if (!m.Success)
					continue;

				Rand r;

				if (m.Groups["die"].Success) {
					int dice = Int32.Parse(m.Groups["dice"].Value);
					int sides = Int32.Parse(m.Groups["sides"].Value);
					r = Rand.Dice(dice, sides);
				} else if (m.Groups["tri"].Success) {
					int center = Int32.Parse(m.Groups["center"].Value);
					int range = Int32.Parse(m.Groups["range"].Value);
					r = Rand.Triangle(center, range);
				} else if (m.Groups["range"].Success) {
					int min = Int32.Parse(m.Groups["min"].Value);
					int max = Int32.Parse(m.Groups["max"].Value);
					r = Rand.Range(min, max);
				} else if (m.Groups["taper"].Success) { // add the taper
					int chance = Int32.Parse(m.Groups["chance"].Value);
					int outOf = Int32.Parse(m.Groups["outof"].Value);

					r = Taper(chance, outOf);
				} else { // fixed
					int value = Int32.Parse(m.Groups["value"].Value);
					r = Rand.Constant(value);
				}

				if (prev == null) {
					prev = r;
					rand = r;
				} else {
					prev.nextRand = r;
					prev = r;
				}				
			}

			return rand;
		}

		public static Rand Constant(int value) {
			return new Rand(
					() => value,
					value, value, value, value.ToString(), true);
		}

		public static Rand Range(int min, int max) {
			Contract.Requires<ArgumentOutOfRangeException>(min <= max);
			return new Rand(
					() => Rng.IntInclusive(min, max),
					min, (min + max) / 2.0f, max, string.Format("{0}-{1}", min, max));
		}

		public static Rand Dice(int dice, int sides) {
			Contract.Requires<ArgumentOutOfRangeException>(dice > 0, "The argument \"dice\" must be greater than zero.");
			Contract.Requires<ArgumentOutOfRangeException>(sides > 0, "The argument \"sides\" must be greater than zero.");
			return new Rand(
					() => Rng.Roll(dice, sides),
					dice, dice * ((1 + sides) / 2.0f), dice * sides + 1, string.Format("{0}d{1}", dice, sides));
		}

		public static Rand Triangle(int center, int range) {
			Contract.Requires<ArgumentOutOfRangeException>(range >= 0, "The argument \"range\" must be zero or greater.");
			return new Rand(
					() => Rng.TriangleInt(center, range),
					center - range, center, center + range + 1, string.Format("{0}t{1}", center, range));
		}

		public static Rand Taper(int chance, int outOf) {
			Contract.Requires<ArgumentOutOfRangeException>(chance > 0, "The chance must be greater than zero.");
			Contract.Requires<ArgumentOutOfRangeException>(chance < outOf, "The chance must be less than the range.");
			Contract.Requires<ArgumentOutOfRangeException>(outOf > 0, "The range must be positive.");
			return new Rand(
					() => Rng.Taper(0, 1, chance, outOf),
					Int32.MinValue,
					(float) chance / (outOf - (float) chance), // sum of geometric series
					Int32.MaxValue,
					string.Format("({0}:{1})", chance, outOf));
		}

		public static Rand Gaussian(int mean, int range, int stddev) {
			Contract.Requires<ArgumentOutOfRangeException>(range >= 0, "The argument \"range\" must be zero or greater.");

			return new Rand(
					() => Rng.GaussianInt(mean, range, stddev),
					mean - range, mean, mean + range + 1,
					String.Format("[μ={0}, r=[{2}-{3}], σ={1}]", mean, stddev, mean - range, mean + range));
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
		/// Gets the (inclusive) minimum of this Random value
		/// </summary>
		public float Mininum {
			get {
				float min = this.min;
				if (nextRand != null)
					min += nextRand.Mininum;

				return min;
			}
		}

		/// <summary>
		/// Gets the average of the values rolled by this Random value.
		/// </summary>
		public float Average {
			get {
				float avrg = this.average;
				if (nextRand != null)
					avrg += nextRand.Average;

				return avrg;
			}
		}

		/// <summary>
		/// Gets the (exclusive) maximum of this Random value
		/// </summary>
		public float Maximum {
			get {
				float max = this.max;
				if (nextRand != null)
					max += nextRand.Maximum;

				return max;
			}
		}

		public override string ToString() {
			string text = this.text;
			if (nextRand != null)
				text += string.Format(" + {0}", nextRand);

			return text;
		}

		public int Roll() {
			int result = rollFunction();
			if (nextRand != null)
				result += nextRand.Roll();

			return result;
		}

		private Rand(Func<int> rollFunction, float min, float average, float max, string text, bool isConstant) {
			Contract.Requires<ArgumentNullException>(rollFunction != null, "rollFunction");
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(text));
			Contract.Requires<ArgumentOutOfRangeException>(min <= average && average <= max);
			this.rollFunction = rollFunction;
			this.min = min;
			this.average = average;
			this.max = max;
			this.text = text;
			this.constant = isConstant;
		}

		private Rand(Func<int> rollFunction, float min, float average, float max, string text)
				: this(rollFunction, min, average, max, text, false) {}

		private static Regex parser;

		private readonly Func<int> rollFunction;
		private readonly float min;
		private readonly float average;
		private readonly float max; // exclusive
		private readonly string text;
		private readonly bool constant;

		// allows rollers to be chained: 2d6 + 3d4 + 1t4...
		private Rand nextRand;

		public static Rand operator +(Rand v1, Rand v2) {
			return new Rand(v2.rollFunction, v2.Mininum, v2.Average, v2.Maximum, v2.text, v2.IsConstant) {nextRand = v1};
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(max >= average && average >= min);
			Contract.Invariant(text != null);
		}
	}
}