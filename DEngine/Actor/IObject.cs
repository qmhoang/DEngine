using DEngine.Core;
using libtcod;

namespace DEngine.Actor {
    /// <summary>
    /// Interfaces for all entities that can be drawn in the game
    /// </summary>
    public interface IObject {
        TCODColor Color { get; }
        char Ascii { get; }
        Point Position { get; set; }
        bool IsVisibleTo(IObject thing);
        ActionResult Move(int dx, int dy);
        ActionResult Move(Point p);
        ActionResult Wait();
        int SightRadius { get; }
        bool Spot(IObject @object);
        bool Spot(Point position);
    }
}
