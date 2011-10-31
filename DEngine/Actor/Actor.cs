using DEngine.Core;
using libtcod;

namespace DEngine.Actor {
    public enum ActionResult {
        Aborted,
        Failed,
        Success,
        SuccessNoTime
    }

    public abstract class Actor : IVisible, ISpot, IGuid, IUpdateable, ITcodDrawable, IMoveable {
        public const int DefaultActionCost = 100;
        public abstract string Name { get; }

        #region IGuid Members

        public long Guid { get; private set; }

        #endregion

        #region IMoveable Members

        public abstract ActionResult Move(int dx, int dy);

        public virtual ActionResult Move(Point p) {
            return Move(p.X, p.Y);
        }

        public virtual ActionResult Wait() {
            ActionPoints += DefaultActionCost;
            return ActionResult.Success;
        }

        #endregion

        #region IVisible Members

        public Point Position { get; set; }
        public abstract bool IsVisibleTo(ISpot thing);

        #endregion

        #region ISpot Members

        public abstract int SightRadius { get; }
        public abstract bool Spot(IVisible @object);
        public abstract bool Spot(Point position);

        #endregion

        #region ITCODDrawable Members

        public abstract char Ascii { get; }
        public abstract TCODColor Color { get; }

        #endregion

        #region IUpdateable Members

        public int ActionPoints { get; set; }
        public abstract int Speed { get; }
        public abstract void Update();
        public abstract bool Dead { get; }
        public abstract void OnDeath();

        #endregion

        
    }
}