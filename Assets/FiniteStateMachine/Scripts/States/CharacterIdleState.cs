namespace FiniteStateMachine.Scripts.States {
    public class CharacterIdleState : CharacterBaseState {
        public CharacterIdleState(StateMachine stateMachine, Character character) : base(stateMachine, character) {
        }

        public override void EnterState() {
        }

        public override void UpdateState() {
            var input = character.intendedMove;
            character.velocity = input * character.moveSpeed;


            if(character.intendDash) {
                stateMachine.ChangeState(character.dashState);
            }
        }

        public override void ExitState() {
        }
    }
}