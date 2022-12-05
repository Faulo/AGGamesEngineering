namespace FiniteStateMachine.Scripts.States {
    public abstract class State {
        protected StateMachine stateMachine;

        protected State(StateMachine stateMachine) {
            this.stateMachine = stateMachine;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}