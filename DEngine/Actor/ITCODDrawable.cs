using libtcod;

namespace DEngine.Actor {
    /// <summary>
    /// Interfaces for all entities that can be drawn in the game
    /// </summary>
    public interface ITcodDrawable {
        TCODColor Color { get; }
        char Ascii { get; }
    }
}
