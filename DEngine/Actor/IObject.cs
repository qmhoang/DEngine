using DEngine.Actor.Components.Graphics;
using DEngine.Core;
using libtcod;

namespace DEngine.Actor {
    /// <summary>
    /// Interfaces for all entities that can be drawn in the game
    /// </summary>
    public interface IObject {        
        Point Position { get; set; }
        RefId RefId { get; }
    }
}
