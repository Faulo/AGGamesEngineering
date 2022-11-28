using System;
using System.Collections;
using UnityEngine;

namespace Coroutines.Scripts {
    public class ObjectFader : MonoBehaviour {

        [SerializeField] private float fadeDurationInSeconds = 3f;
        [SerializeField] private Renderer rend;
        [SerializeField] private AnimationCurve fadeCurve;
        private Coroutine currentCo;

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
               currentCo = StartCoroutine(Fade_Co());
            }

            if(Input.GetKeyDown(KeyCode.D)) {
                currentCo = StartCoroutine(DanielFade_Co());
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                if(currentCo != null) {
                    StopCoroutine(currentCo);
                    //currentCo = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Debug.Log(currentCo);
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

            for (float alpha = 1f; alpha >= 0; alpha -= .1f)
            {
                c.a = alpha;
                rend.material.color = c;
                yield return new WaitForSeconds(.1f);
            }
            c.a = 0f;
            rend.material.color = c;
        }

        private IEnumerator DanielFade_Co() {

            var c = rend.material.color;

            for (float t = 0f; t <= fadeDurationInSeconds; t += Time.deltaTime) {
                c.a = fadeCurve.Evaluate(t/fadeDurationInSeconds);
                rend.material.color = c;
                yield return null;
            }
        }
    }
}