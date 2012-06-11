using System;

namespace DEngine.Extensions {
    public static class RandomExtentions {
        public static readonly Random Random = new Random(0);

        /// <summary>
        /// Given a percentage chance, do you succeed or fail?
        /// </summary>
        public static bool Chance(this Random random, double percent) {
            return random.NextDouble() <= percent;
        }
    }
}
