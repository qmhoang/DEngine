using DEngine.Core;

namespace DEngine.Actor {
    public interface IMoveable {
        ActionResult Move(int dx, int dy);
        ActionResult Move(Point p);
        ActionResult Wait();
    }
}