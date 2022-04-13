using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace AGGE.CharacterController3D {
    public class PostProcessingController : MonoBehaviour {
        [SerializeField]
        Transform depthOfFieldTarget;

        [SerializeField]
        VolumeProfile attachedProfile;

        DepthOfField depthOfFieldOverride {
            get {
                if (m_depthOfFieldOverride) {
                    return m_depthOfFieldOverride;
                }
                if (attachedProfile.TryGet(out m_depthOfFieldOverride)) {
                    return m_depthOfFieldOverride;
                }
                m_depthOfFieldOverride = attachedProfile.Add<DepthOfField>();
                return m_depthOfFieldOverride;
            }
        }
        DepthOfField m_depthOfFieldOverride;

        protected void Start() {
            depthOfFieldOverride.SetAllOverridesTo(false);
            depthOfFieldOverride.mode.value = DepthOfFieldMode.Bokeh;
            depthOfFieldOverride.mode.overrideState = true;
            depthOfFieldOverride.focusDistance.overrideState = true;
        }
        protected void Update() {
            if (depthOfFieldTarget) {
                float distance = Vector3.Distance(depthOfFieldTarget.position, transform.position);

                depthOfFieldOverride.focusDistance.value = distance;
            }
        }
    }
}