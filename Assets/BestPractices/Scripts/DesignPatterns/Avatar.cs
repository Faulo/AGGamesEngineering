using UnityEngine;

namespace AGGE.BestPractices.DesignPatterns {
    public class Avatar : ICharacter {
        public IMover mover { get; set; }
        public float speed { get; set; }
        public Transform transform { get; set; }
        public Avatar(Transform transform, IMover mover = null) {
            this.transform = transform;
            if (mover == null) {
                if (transform.TryGetComponent<Rigidbody>(out var rigidbody)) {
                    mover = new RigidbodyMover { rigidbody = rigidbody };
                } else {
                    mover = new TransformMover { transform = transform };
                }
            }
            this.mover = mover;
        }
        public void Start() {
        }
        public void Update() {
            mover.Move(transform.forward, speed);
        }
    }

    class TestMover : IMover {
        public Vector3 direction;
        public void Move(Vector3 direction, float speed) {
            this.direction = direction;
        }
    }
}