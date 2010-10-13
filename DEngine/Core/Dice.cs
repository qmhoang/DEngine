using System;
using DEngine.Extensions;

namespace DEngine.Core {
    public struct Dice {
        public static Dice Invalid = new Dice(-1, -1, -1, -1);

        public int Min;
        public int ExclusiveMax;

        public Dice(int nums, int diceFaces)
            : this((short)nums, (short)diceFaces, (short)0, (short)1) { }

        public Dice(int nums, int diceFaces, int toAdd)
            : this((short)nums, (short)diceFaces, (short)toAdd, (short)1) { }

        public Dice(int nums, int diceFaces, int toAdd, int multiplier)
            : this((short)nums, (short)diceFaces, (short)toAdd, (short)multiplier) { }

        public Dice(short nums, short diceFaces)
            : this(nums, diceFaces, (short)0, (short)1) { }

        public Dice(short nums, short diceFaces, short toAdd)
            : this(nums, diceFaces, toAdd, (short)1) { }

        public Dice(short nums, short diceFaces, short toAdd, short multiplier) {
            Min = (nums + toAdd) * multiplier;
            ExclusiveMax = (nums * diceFaces + toAdd) * multiplier + 1;
        }

        public Dice(int inclusiveMax) {
            Min = 0;
            ExclusiveMax = inclusiveMax + 1;
        }

        public static bool operator ==(Dice d1, Dice d2) {
            return d1.Min == d2.Min && d1.ExclusiveMax == d2.ExclusiveMax;
        }

        public static bool operator !=(Dice d1, Dice d2) {
            return !(d1 == d2);
        }

        // 1d4, 1d8 + 1, 3d20 + 2 * 2, [4-16], 
        public static Dice Parse(string s) {
            int dices, faces, add, mult;

            if (s.Contains("-")) {
                string[] s4 = s.Split('-');
                int min = Int32.Parse(s4[0]);
                int max = Int32.Parse(s4[1]);

                return new Dice(1, max - min + 1, min - 1);
            }

            string[] s1 = s.Split('*');
            mult = s1.Length == 1 ? 1 : Int32.Parse(s1[1]);
            string[] s2 = s1[0].Split('+');
            add = s2.Length == 1 ? 0 : Int32.Parse(s2[1]);

            string[] s3 = s2[0].Split('d');
            if (s3.Length == 1) {
                dices = 1;
                faces = 1;
                add = Int32.Parse(s3[0]);
            } else {
                dices = Int32.Parse(s3[0]);
                faces = Int32.Parse(s3[1]);
            }

            return new Dice(dices, faces, add, mult);
        }

        /// <summary>
        /// Inclusive max
        /// </summary>
        public int InclusiveMax {
            get { return ExclusiveMax - 1; }
        }

        public int Roll() {
            return RandomExtentions.Random.Next(Min, ExclusiveMax);
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
            return InclusiveMax;
        }

        public override string ToString() {
            return string.Format("{0}-{1}", Min, InclusiveMax);
        }

        public bool Equals(Dice other) {
            return other.Min == Min && other.ExclusiveMax == ExclusiveMax;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(Dice))
                return false;
            return Equals((Dice)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Min * 397) ^ ExclusiveMax;
            }
        }
    }
}