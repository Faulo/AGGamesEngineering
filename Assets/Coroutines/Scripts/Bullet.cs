using System;
using System.Collections;
using UnityEngine;

namespace Coroutines.Scripts {
    public class Bullet : MonoBehaviour {
        [SerializeField] private float delayInSeconds;
        
        protected void Start() {
            StartCoroutine(ActionAfterSeconds_Co(() => Destroy(gameObject), delayInSeconds));
        }

        private IEnumerator ActionAfterSeconds_Co(Action callback, float delay) {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }
    }
}