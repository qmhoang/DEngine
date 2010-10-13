using DEngine.Core;
using libtcod;

namespace DEngine.States
{
    public abstract class Screen
    {
        public Point ScreenPos { get; private set; }
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
            this.ScreenPos = screenPos;
            this.screen = screen;
        }

        public abstract void Update();
        public abstract void Draw();

    }
}
