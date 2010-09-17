using DEngine.Core;
using libtcod;

namespace DEngine.States
{
    abstract class Screen
    {
        public Point ScreenPosition { get; private set; }
        protected readonly TCODConsole screen;        

        public int Height {
            get {
                return screen.getHeight();
            }
        }
        public int Width { 
            get {
                return screen.getWidth();    
            } 
        }

        protected Screen(Point screenPos, TCODConsole screen) {
            this.ScreenPosition = screenPos;
            this.screen = screen;
        }

        public abstract void Update();
        public abstract void Draw();

    }
}
