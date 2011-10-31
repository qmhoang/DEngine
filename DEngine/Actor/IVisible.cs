using DEngine.Core;

namespace DEngine.Actor {
    public interface IVisible {
        Point Position { get; set; }
        bool IsVisibleTo(ISpot thing);
    }
}
