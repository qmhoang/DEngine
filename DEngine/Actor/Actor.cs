using System.Collections.Generic;
using DEngine.Actor.Components.Graphics;
using DEngine.Core;
using libtcod;

namespace DEngine.Actor {
    public enum ActionResult {
        Aborted,
        Failed,
        Success,
        SuccessNoTime
    }


    public abstract class Actor : IEntity, IObject {
        public abstract string Name { get; }
        public string RefId { get; protected set; }
        public UniqueId Uid { get; protected set; }

        public abstract Image Image { get; set; }
        public Point Position { get; set; }

        public virtual ActionResult Move(Point p) {
            return Move(p.X, p.Y);
        }

        public abstract ActionResult Move(int dx, int dy);
        public abstract ActionResult Wait();

        public bool IsNear(int x, int y, int radius) {
            return IsNear(new Point(x, y), radius);
        }

        public bool IsNear(Point point, int radius) {
            return point.IsInCircle(Position, radius);
        }

        public abstract int SightRadius { get; }

        public bool IsVisibleTo(Actor actor) {
            return actor.CanSpot(this);
        }
        public bool HasLineOfSight(Actor target) {
            return HasLineOfSight(target.Position);
        }
        public abstract bool HasLineOfSight(Point position);
        public abstract bool CanSpot(Actor target);

        public bool Updateable { get { return ActionPoints > 0; } }        
        public int ActionPoints { get; set; }
        public abstract int Speed { get; }
        public abstract void Update();
        public abstract bool Dead { get; }
        public abstract void OnDeath();

    }
}