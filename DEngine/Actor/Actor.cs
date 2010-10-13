using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using DEngine.Core.Interfaces;
using libtcod;

namespace DEngine.Actor {
    public enum ActionResult {
        Aborted,
        Failed,
        Success,
        SuccessNoTime
    }

    public abstract class Actor : INoticeable, ISeeable, IGuid, IUpdateable, IDrawable, IMoveable {
        public const int DefaultWalkCost = 100;

        public long Guid { get; private set; }
        public Point Position { get; set; }

        public abstract string Name { get; }
        public abstract char Ascii { get; }
        public abstract TCODColor Color { get; }
        public abstract int SightRadius { get; }

        public int ActionPoints { get; set; }
        public abstract int Speed { get; }
        public abstract void Update();
        public abstract bool Dead { get; }
        public abstract void OnDeath();

        public abstract bool IsVisibleTo(ISeeable thing);
        public abstract bool CanSee(INoticeable thing);
        public abstract bool CanSee(Point position);

        public abstract ActionResult Move(int dx, int dy);
        public virtual ActionResult Move(Point p) {
            return Move(p.X, p.Y);
        }

        public virtual ActionResult Wait() {
            ActionPoints += DefaultWalkCost;
            return ActionResult.Success;
        }
    }
}
