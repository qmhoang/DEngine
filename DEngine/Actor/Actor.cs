using System.Collections.Generic;
using DEngine.Core;
using libtcod;

namespace DEngine.Actor {
    public enum ActionResult {
        Aborted,
        Failed,
        Success,
        SuccessNoTime
    }


    public abstract class Actor : IUniqueId, IEntity, IObject {
        public const int DefaultActionCost = 100;

        public abstract string Name { get; }
        public long UniqueId { get; protected set; }
        public abstract ActionResult Move(int dx, int dy);
        public virtual ActionResult Move(Point p) {
            return Move(p.X, p.Y);
        }
        public virtual ActionResult Wait() {
            ActionPoints += DefaultActionCost;
            return ActionResult.Success;
        }
        
        public Point Position { get; set; }

        public bool IsVisibleTo(Actor actor) {
            return actor.CanSpot(this);
        }

        public bool IsNear(int x, int y, int radius) {
            return IsNear(new Point(x, y), radius);
        }

        public bool IsNear(Point point, int radius) {
            return point.IsInCircle(Position, radius);
        }

        public abstract int SightRadius { get; }

        public bool HasLineOfSight(IObject @object) {
            return HasLineOfSight(@object.Position);
        }
        public abstract bool HasLineOfSight(Point position);

        public abstract bool CanSpot(IObject @object);

        public abstract char Ascii { get; }
        public abstract Color Color { get; }

        public bool Updateable { get { return ActionPoints > 0; } }        
        public int ActionPoints { get; set; }
        public abstract int Speed { get; }
        public abstract void Update();
        public abstract bool Dead { get; }
        public abstract void OnDeath();

    }
}