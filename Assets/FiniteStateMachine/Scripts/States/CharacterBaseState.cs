namespace FiniteStateMachine.Scripts.States {
    public abstract class CharacterBaseState : State {
        protected Character character;

        protected CharacterBaseState(StateMachine stateMachine, Character character) : base(stateMachine) {
            this.character = character;
        }

        public abstract override void EnterState();
        public abstract override void UpdateState();
        public abstract override void ExitState();
    }
}