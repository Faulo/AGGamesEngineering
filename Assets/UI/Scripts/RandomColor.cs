using UnityEngine;
using UnityEngine.UI;

namespace AGGE.UI {
    public class RandomColor : MonoBehaviour {
        [SerializeField]
        Image image = default;

        [SerializeField]
        Color color = default;

        protected void Awake() {
            SetupComponents();
        }

        protected void OnValidate() {
            SetupComponents();
        }
        void Start() {
            SetupComponents();
        }

        void SetupComponents() {
            if (!image) {
                image = GetComponent<Image>();
            }
            color = UnityEngine.Random.ColorHSV();
            image.color = color;
        }

    }
}