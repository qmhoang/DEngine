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


    public abstract class AbstractActor : IGuid, IEntity, IObject {
        public const int DefaultActionCost = 100;
        public abstract string Name { get; }


        public long Guid { get; protected set; }
        public abstract ActionResult Move(int dx, int dy);
        public virtual ActionResult Move(Point p) {
            return Move(p.X, p.Y);
        }
        public virtual ActionResult Wait() {
            ActionPoints += DefaultActionCost;
            return ActionResult.Success;
        }
        

        public Point Position { get; set; }

        public bool IsVisibleTo(AbstractActor actor) {
            return actor.Spot(this);
        }
        

        public abstract int SightRadius { get; }

        public bool Spot(IObject @object) {
            return Spot(@object.Position);
        }
        public abstract bool Spot(Point position);

        public abstract char Ascii { get; }
        public abstract Color Color { get; }

        public int ActionPoints { get; set; }
        public abstract int Speed { get; }
        public abstract void Update();
        public abstract bool Dead { get; }
        public abstract void OnDeath();

    }
}