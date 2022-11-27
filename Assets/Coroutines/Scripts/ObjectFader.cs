using System;
using System.Collections;
using UnityEngine;

namespace Coroutines.Scripts {
    public class ObjectFader : MonoBehaviour {

        [SerializeField] private Renderer rend;

        private void OnValidate() {
            if (!rend) {
                TryGetComponent(out rend);
            }
        }

        protected void Update() {
            if (Input.GetKeyDown(KeyCode.F)) {
                Fade();
            }

            if (Input.GetKeyDown(KeyCode.C)) {
               StartCoroutine(Fade_Co());
            }
        }

        private void Fade() {
            var c = rend.material.color;
            for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
            {
                c.a = alpha;
                rend.material.color = c;
            }
        }
        
        private IEnumerator Fade_Co()
        {
            var c = rend.material.color;
            for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
            {
                c.a = alpha;
                rend.material.color = c;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}