using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace AGGE.GameOfLife {
    public class CellController : MonoBehaviour {
        [SerializeField]
        Animator attachedAnimator = default;
        [SerializeField]
        public CellController[] neighbors = Array.Empty<CellController>();
        public bool isAlive {
            get => attachedAnimator.GetBool(nameof(isAlive));
        }

        protected IEnumerator Start() {
            if (UnityEngine.Random.value > 0.5f) {
                attachedAnimator.CrossFade("Alive", 0);
            } else {
                attachedAnimator.CrossFade("Dead", 0);
            }
            yield return null;
            neighbors = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1))
                .SelectMany(collider => collider.GetComponents<CellController>())
                .Where(cell => cell != this)
                .ToArray();
        }
    }
}