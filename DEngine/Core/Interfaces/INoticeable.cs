namespace DEngine.Core.Interfaces {
    public interface INoticeable {
        Point Position { get; set; }
        bool IsVisibleTo(ISeeable thing);
    }
}
