using UnityEngine;

internal class TransformMover : IMover
{
    public Transform transform;
    public void Move(Vector3 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
internal class RigidbodyMover : IMover
{
    public Rigidbody rigidbody;
    public void Move(Vector3 direction, float speed)
    {
        rigidbody.velocity = direction * speed;
    }
}
