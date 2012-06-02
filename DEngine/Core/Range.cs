using System;
using DEngine.Extensions;

namespace DEngine.Core {
    public interface IRand {
        int Roll();
    }

    public struct Range : IRand {
        public readonly int Min;
        public readonly int Max;
        public int InclusiveMax { get { return Max - 1; } }

        public Range(int min, int max) {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            Max = max;
            Min = min;
        }

        public bool IsInRange(int value) {
            return (Min <= value) && (value <= Max);
        }

        public int Roll() {
            return RandomExtentions.Random.Next(Min, Max);
        }
    }

    public struct Dice : IRand {
        public static Dice Invalid = new Dice(-1, -1, -1, -1);

        readonly public int Nums, DiceFaces, Modifier, Multiplier;

        public Dice(int nums, int diceFaces)
            : this(nums, diceFaces, 0, 1) { }

        public Dice(int nums, int diceFaces, int modifier)
            : this(nums, diceFaces, modifier, 1) { }

        public Dice(int nums, int diceFaces, int modifier, int multiplier) {
            Nums = nums;
            DiceFaces = diceFaces;
            Modifier = modifier;
            Multiplier = multiplier;
        }

        // 1d4, 1d8 + 1, 3d20 + 2 * 2, 
        public Dice(string s) {
            int nums, diceFaces, modifier, multiplier;

            //            if (s.Contains("-")) {
            //                string[] s4 = s.Split('-');
            //                int min = Int32.Parse(s4[0]);
            //                int max = Int32.Parse(s4[1]);
            //
            //                return new Dice(1, max - min + 1, min - 1);
            //            }

            string[] s1 = s.Split('*');
            multiplier = s1.Length == 1 ? 1 : Int32.Parse(s1[1]);
            string[] s2 = s1[0].Split(new char[] { '+', '-' });
            modifier = s2.Length == 1 ? 0 : Int32.Parse(s2[1]);

            string[] s3 = s2[0].Split('d');
            if (s3.Length == 1) {
                nums = 1;
                diceFaces = 1;
                modifier = Int32.Parse(s3[0]);
            } else {
                nums = Int32.Parse(s3[0]);
                diceFaces = Int32.Parse(s3[1]);
            }

            Nums = nums;
            DiceFaces = diceFaces;
            Modifier = s.Contains("-") ? -modifier : modifier;
            Multiplier = multiplier;
        }

        public int Roll() {
            short total = 0;
            for (short i = 0; i < Nums; i++) {
                total += (short)(RandomExtentions.Random.Next(DiceFaces) + 1);
            }
            return Multiplier * (total + Modifier);
        }

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Roll x number of y-sided dices.  Take the highest value.
        /// </summary>
        /// <param name="num">x</param>
        /// <param name="face">y</param>
        /// <returns>Highest die roll</returns>
        public static int RollXdYH(int num, int face) {
            // ReSharper restore InconsistentNaming            
            int max = 0;
            for (int i = 0; i < num; i++) {
                max = Math.Max(RandomExtentions.Random.Next(face), max);
            }
            return max + 1;
        }

        /// <summary>
        /// Roll x number of y-sided dices.  Take the lowest value.
        /// </summary>
        /// <param name="num">x</param>
        /// <param name="face">y</param>
        /// <returns>Lowest die roll</returns>
        public static int RollXdYL(int num, int face) {
            int min = Int32.MaxValue;
            for (int i = 0; i < num; i++) {
                min = Math.Min(RandomExtentions.Random.Next(face), min);
            }
            return min + 1;
        }

        public int RollMax() {
            if (Multiplier != 0)
                return (int)Math.Round((double)(Multiplier * (DiceFaces * Nums)) + Modifier);
            else
                return (DiceFaces * Nums) + Modifier;
        }

        public override string ToString() {
            bool hasMult = Multiplier != 1;
            bool hasConstant = Modifier != 0;

            string multiplierFrontString = hasMult ? Multiplier + "*" : String.Empty;
            string frontParen = hasMult || hasConstant ? "(" : String.Empty;
            string endParen = hasMult || hasConstant ? ")" : String.Empty;
            string constantSign = Modifier >= 0 ? "+" : String.Empty; // Minus will come with number
            string constantEnd = hasConstant ? constantSign + Modifier : String.Empty;

            // 1d1 doesn't look nice
            if (!hasMult && !hasConstant && Nums == 1 && DiceFaces == 1)
                return "1";

            return string.Format("{0}{1}{2}d{3}{4}{5}", multiplierFrontString, frontParen, Nums,
                                 DiceFaces, constantEnd, endParen);
        }

        public override int GetHashCode() {
            unchecked {
                int result = Nums.GetHashCode();
                result = (result * 397) ^ DiceFaces.GetHashCode();
                result = (result * 397) ^ Modifier.GetHashCode();
                result = (result * 397) ^ Multiplier.GetHashCode();
                return result;
            }
        }
    }
}