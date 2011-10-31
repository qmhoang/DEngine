namespace DEngine.Actor {
    /// <summary>
    /// Global unique id for every game element in the universe
    /// </summary>
    public interface IGuid {
        long Guid { get; }
    }
}
