using UnityEngine;

namespace AGGE.CharacterController2D {
    public class CameraController : MonoBehaviour {

        public float speedX;
        public float speedY;
        public Transform target;

        protected void Awake() {
            if (target == null) {
                target = FindObjectOfType<Player>().transform;
            }

            var startPos = transform.position;
            var targetPos = target.position;

            if (speedX > 0) {
                startPos.x = targetPos.x;
            }

            if (speedY > 0) {
                startPos.y = targetPos.y;
            }

            transform.position = startPos;
        }

        protected void Update() {
            var pos = transform.position;
            var diff = target.position - pos;

            if (speedX > 0 && Mathf.Abs(diff.x) > 0.02f) {
                pos.x += diff.x * Time.deltaTime * speedX;
            }

            if (speedY > 0 && Mathf.Abs(diff.y) > 0.02f) {
                pos.y += diff.y * Time.deltaTime * speedY;
            }

            transform.position = pos;
        }
    }
}