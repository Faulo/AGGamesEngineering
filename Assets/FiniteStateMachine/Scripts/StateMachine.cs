using FiniteStateMachine.Scripts.States;

namespace FiniteStateMachine.Scripts {
    public class StateMachine {
        public State currentState;

        public void Initialize(State startingState) {
            currentState = startingState;
            currentState.EnterState();
        }

        public void ChangeState(State newState) {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}