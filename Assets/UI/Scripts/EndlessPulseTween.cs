using UnityEngine;

namespace AGGE.UI {
    public class EndlessPulseTween : MonoBehaviour {
        [SerializeField]
        GameObject objectToWiggle = default;
        [SerializeField, Range(0, 10)]
        float wiggleTime = 1f;
        [SerializeField]
        LeanTweenType easeType = LeanTweenType.easeInOutBack;

        protected void Start() {
            WiggleNow();
        }
        void WiggleNow() {
            objectToWiggle.transform.localScale = Vector3.zero;
            LeanTween.scale(objectToWiggle, Vector3.one, wiggleTime).setEase(easeType).setLoopPingPong(-1);
        }
    }
}