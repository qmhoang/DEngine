using DEngine.Core;

namespace DEngine.Actor {
    public interface ISpot {
        int SightRadius { get; }
        bool Spot(IVisible @object);
        bool Spot(Point position);
    }
}