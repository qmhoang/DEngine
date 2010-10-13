namespace DEngine.Core {
    public class GuidFactory {
        private static long id = 0;

        public static long GenerateGuid() {
            return id++;
        }
    }
}