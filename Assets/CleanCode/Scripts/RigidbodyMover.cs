using UnityEngine;

class RigidbodyMover : IMover {
    public Rigidbody rigidbody;
    public void Move(Vector3 direction, float speed) {
        rigidbody.velocity = direction * speed;
    }
}
