using System.Collections;
using Slothsoft.UnityExtensions;
using TMPro;
using UnityEngine;

namespace AGGE.Input {
    public abstract class InputBase : MonoBehaviour, IUpdateModeMessages {
        [SerializeField]
        internal GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;

        protected virtual void Start() {
            if (!referencedObject) {
                referencedObject = gameObject;
            }

            if (transform.TryGetComponentInChildren<TMP_Text>(out var text)) {
                text.text = gameObject.name;
            }

            StartCoroutine(WaitForFixedUpdate_Co());
            StartCoroutine(WaitForUpdate_Co());
            StartCoroutine(WaitForEndOfFrame_Co());
        }

        IEnumerator WaitForFixedUpdate_Co() {
            var wait = new WaitForFixedUpdate();
            while (true) {
                yield return wait;
                TryProcessUpdate(EUpdateMode.WaitForFixedUpdate);
            }
        }

        IEnumerator WaitForUpdate_Co() {
            object wait = default;
            while (true) {
                yield return wait;
                TryProcessUpdate(EUpdateMode.WaitForUpdate);
            }
        }

        IEnumerator WaitForEndOfFrame_Co() {
            var wait = new WaitForEndOfFrame();
            while (true) {
                yield return wait;
                TryProcessUpdate(EUpdateMode.WaitForEndOfFrame);
            }
        }

        bool _isPressed;
        internal bool isPressed {
            get => _isPressed;
            set {
                _isPressed = value;

                referencedObject.transform.position = referencedObject.transform.position.WithZ(value ? 1 : 0);

                if (referencedObject.TryGetComponent<Renderer>(out var renderer)) {
                    renderer.sharedMaterial = value
                        ? lightMaterial
                        : darkMaterial;
                }
            }
        }

        internal Vector2 velocity;

        protected virtual void FixedUpdate() {
            TryProcessUpdate(EUpdateMode.FixedUpdate);

            referencedObject.transform.Translate(velocity.SwizzleXY() * Time.deltaTime);
        }

        protected virtual void Update() {
            TryProcessUpdate(EUpdateMode.Update);
        }

        protected virtual void LateUpdate() {
            TryProcessUpdate(EUpdateMode.LateUpdate);
        }

        [SerializeField]
        EUpdateMode mode = EUpdateMode.Update;

        public void OnUpdateMode(EUpdateMode mode) {
            this.mode = mode;
        }

        void TryProcessUpdate(EUpdateMode mode) {
            if (this.mode.HasFlag(mode)) {
                ProcessInput();
            }
        }

        protected abstract void ProcessInput();
    }
}