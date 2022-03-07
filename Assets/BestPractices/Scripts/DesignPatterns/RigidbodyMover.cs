using UnityEngine;

namespace AGGE.BestPractices.DesignPatterns {
    class RigidbodyMover : IMover {
        public Rigidbody rigidbody;
        public void Move(Vector3 direction, float speed) {
            rigidbody.velocity = direction * speed;
        }
    }
}