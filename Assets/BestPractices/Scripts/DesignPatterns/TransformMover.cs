using UnityEngine;

namespace AGGE.BestPractices.DesignPatterns {
    class TransformMover : IMover {
        public Transform transform;
        public void Move(Vector3 direction, float speed) {
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}