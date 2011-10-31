using System;

namespace DEngine.Extensions {
    class RandomExtentions {
        public static readonly Random Random = new Random(0);

        /// <summary>
        /// Given a percentage chance, do you succeed or fail?
        /// </summary>
        /// <param name="percent">Percent to succeed</param>
        /// <returns>True if success, false if failure</returns>
        public static bool Chance(double percent) {
            return Random.NextDouble() <= percent;
        }
    }
}
