using FiniteStateMachine.Scripts.States;
using UnityEngine;
using UnityEngine.InputSystem;
using Slothsoft.UnityExtensions;

namespace FiniteStateMachine.Scripts {
    public class Character : MonoBehaviour {
        [SerializeField] private CharacterController controller;
        public float moveSpeed = 20f;
        public float dashSpeed = 100f;

        private StateMachine stateMachine;
        public CharacterIdleState idleState;
        public CharacterDashState dashState;

        public Vector3 intendedMove;
        public Vector3 velocity;

        public bool intendDash;

        private Controls controls;

        protected void Start() {
            stateMachine = new StateMachine();
            idleState = new CharacterIdleState(stateMachine, this);
            dashState = new CharacterDashState(stateMachine, this);
            stateMachine.Initialize(idleState);
            controls = new Controls();
            controls.Enable();
        }

        protected void Update() {
            intendedMove = controls.Player.Move.ReadValue<Vector2>().SwizzleXZ();
            intendDash = controls.Player.Dash.IsPressed();

            stateMachine.currentState.UpdateState();
        }

        protected void FixedUpdate() {
            controller.Move(velocity * Time.deltaTime);
        }

        protected void OnValidate() {
            if(!controller) {
                TryGetComponent(out controller);
            }
        }
    }
}