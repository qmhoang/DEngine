using System.Collections.Generic;

namespace DEngine.States
{
    abstract class MultiScreenState : GameState
    {
        protected List<Screen> Screens;

        protected MultiScreenState() {
            Screens = new List<Screen>();            
        }

        public override void Update() {
            foreach (var screen in Screens) {
                screen.Update();
            }
        }

        public override void Draw() {
            foreach (var screen in Screens) {
                screen.Draw();
            }
        }
    }
}
