using UnityEngine;

namespace AGGE.UI {
    public class WiggleTween : MonoBehaviour {
        [SerializeField]
        GameObject objectToWiggle = default;
        [SerializeField, Range(0, 10)]
        float wiggleTime = 1f;
        [SerializeField]
        LeanTweenType easeType = LeanTweenType.easeOutElastic;
        [SerializeField]
        bool wiggleNow = true;

        protected void Start() {
            WiggleNow();
        }

        protected void Update() {
            WiggleNow();
        }
        void WiggleNow() {
            if (wiggleNow) {
                objectToWiggle.transform.localScale = Vector3.zero;
                LeanTween.scale(objectToWiggle, Vector2.one, wiggleTime).setEase(easeType);
                wiggleNow = false;
            }
        }
    }
}