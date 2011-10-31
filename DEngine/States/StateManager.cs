using System.Collections.Generic;
using libtcod;

namespace DEngine.States {
    public abstract class GameState {

        public bool IsPopup { get; set; }
        public bool HasFocus { get; set; }

        protected GameState() {}

        public abstract void HandleInput();
        public abstract void Update();
        public abstract void Draw();
        public virtual void Shutdown() {            
            StateManager.Instance.PopState();            
        }
    }

    public class StateManager {
        private readonly Stack<GameState> stateStack;

        public GameState CurrentState
        {
            get { return stateStack.Peek(); }
        }

        private static StateManager instance;

        private StateManager() {
            
            stateStack = new Stack<GameState>();          
        }

        public static StateManager Instance {
            get {
                if (instance == null)
                    instance = new StateManager();
                return instance;
            }
        }

        public void AddState(GameState newState) {
            stateStack.Push(newState);
        }

        public void PopState() {
            stateStack.Pop();
        }

        public void ChangeState(GameState newState) {
            CurrentState.Shutdown();
            stateStack.Push(newState);
        }

        public void Update()
        {
            if (stateStack.Count > 0)
            {
                CurrentState.HandleInput();
                foreach (var state in stateStack)
                {
                    state.Update();
                    if (state.HasFocus)     // if we have focus, the other states do not update
                        break;
                }
            }
        }

        public void Draw() {
            if (stateStack.Count > 0) {
                Stack<GameState> reverse = new Stack<GameState>();

                foreach (var state in stateStack) {
                    reverse.Push(state);
                    if (!state.IsPopup)
                        break;
                }

                TCODConsole.root.clear();
                foreach (var state in reverse) 
                    state.Draw();                
                TCODConsole.flush();
            }
        }
    }
}