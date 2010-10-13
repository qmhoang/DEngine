using libtcod;

namespace DEngine.Core.Interfaces {
    /// <summary>
    /// Interfaces for all entities that can be drawn in the game
    /// </summary>
    public interface IDrawable {
        TCODColor Color { get; }
        char Ascii { get; }
    }
}
