using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Actor;

namespace DEngine.Core {
    class World {
        public static readonly int DefaultTurnSpeed = 100;
        public static readonly int TurnLengthInSeconds = 3;

        private static World instance;

        public List<string> MessageBuffer { get; private set; }

        private readonly List<IUpdateable> updateables;
        private readonly List<IUpdateable> toAdds;
        private readonly List<IUpdateable> toRemove;

        public Calendar Calendar { get; private set; }

        public static World Instance {
            get {
                if (instance == null)
                    throw new Exception("World hasn't been created.");
                return instance;
            }
        }

        public World() {
            Calendar = new Calendar();

            updateables = new List<IUpdateable> { Calendar };
            toAdds = new List<IUpdateable>();
            toRemove = new List<IUpdateable>();
            MessageBuffer = new List<string>();

        }

        public void AddUpdateableObject(IUpdateable i) {
            toAdds.Add(i);
        }

        public void RemoveUpdateableOjects(IUpdateable i) {
            toRemove.Add(i);
        }

        public void Update() {
            foreach (var updateable in toRemove) {
                updateables.Remove(updateable);
            }

            updateables.AddRange(toAdds);

            toAdds.Clear();
            toRemove.Clear();

            // update everything while the player cannot act)););
//            while (Player.ActionPoints > 0) {
//                foreach (var a in updateables) {
//                    a.ActionPoints -= a.Speed;
//                    if (a.ActionPoints <= 0 && !a.Dead)
//                        a.Update();
//                }
//                Player.Update();
//                Player.ActionPoints -= Player.Speed;
//            }

            updateables.RemoveAll(actor => actor.Dead);
        }    
    }
}
