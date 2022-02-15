using UnityEngine;
using UnityEngine.VFX;

namespace AGGE.VisualEffectsGraph {
    public class ParticlesController : MonoBehaviour {
        [SerializeField]
        VisualEffect attachedEffect;

        protected void OnValidate() {
            if (!attachedEffect) {
                TryGetComponent(out attachedEffect);
            }
        }
    }
}
