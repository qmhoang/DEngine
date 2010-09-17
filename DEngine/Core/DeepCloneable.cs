namespace DEngine.Core {
    public interface IDeepCloneable<out T> {
        T Clone();
    }
}