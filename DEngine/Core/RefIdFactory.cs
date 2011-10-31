namespace DEngine.Core {
    public class RefIdFactory {
        private static long id = 0;

        public static long GenerateRefId() {
            return id++;
        }
    }
}