using System;

namespace DEngine.Extensions {
    class RandomExtentions {
        public static readonly Random Random = new Random(0);

        public static bool Chance(double percent) {
            return Random.NextDouble() <= percent;
        }
    }
}
