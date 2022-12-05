using UnityEngine;

namespace FiniteStateMachine.Scripts.States {
    public class CharacterDashState : CharacterBaseState {
        private float timer;

        public CharacterDashState(StateMachine stateMachine, Character character) : base(stateMachine, character) {
        }

        public override void EnterState() {
           timer = 0.5f;
           character.velocity = character.intendedMove.normalized * character.dashSpeed;
        }

        public override void ExitState() {
        
        }

        public override void UpdateState() {
            timer -= Time.deltaTime;

            if(timer <= 0f) {
                stateMachine.ChangeState(character.idleState);
            }
        }
    }
}