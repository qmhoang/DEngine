using DEngine.Core;
using libtcod;

namespace DEngine.States
{
    public abstract class Screen
    {
        public Point ConsolePosition { get; private set; }
        protected readonly TCODConsole Console;        

        public int Height {
            get {
                return Console.getHeight();
            }
        }
        public int Width { 
            get {
                return Console.getWidth();    
            } 
        }

        protected Screen(Point screenPos, TCODConsole console) {
            this.ConsolePosition = screenPos;
            this.Console = console;
        }

        public abstract void Update();
        public abstract void Draw();

    }
}
