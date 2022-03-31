using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.UI {
    public class RaycastController : MonoBehaviour {
        [SerializeField]
        TextMeshProUGUI mousePositionField;
        [SerializeField]
        Camera attachedCamera;

        [SerializeField]
        LayerMask groundLayer;
        [SerializeField]
        LayerMask cubeLayer;

        public Vector2 mousePosition => Mouse.current.position.ReadValue();

        Vector3 mousePosition3D {
            get => m_mousePosition3D;
            set {
                lastMousePosition3D = mousePosition3D;
                m_mousePosition3D = value;
            }
        }
        Vector3 m_mousePosition3D;
        Vector3 lastMousePosition3D;
        Vector3 deltaMousePosition3D => mousePosition3D - lastMousePosition3D;

        protected void Start() {
        }

        GameObject selectedGameObject {
            get => selectedGameObjectCache;
            set {
                if (selectedGameObjectCache != value) {
                    if (selectedGameObjectCache) {
                        selectedGameObjectCache.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.gray);
                        selectedGameObjectCache.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    selectedGameObjectCache = value;
                    if (selectedGameObjectCache) {
                        selectedGameObjectCache.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
                        selectedGameObjectCache.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
        }
        GameObject selectedGameObjectCache;

        protected void Update() {


            //mousePositionField.text = Vector2Int.RoundToInt(mousePosition).ToString();

            var ray = attachedCamera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, attachedCamera.transform.position.z));

            if (Physics.Raycast(ray, out var info, float.PositiveInfinity, groundLayer)) {
                mousePosition3D = info.point;
                mousePositionField.text = deltaMousePosition3D.ToString();
            }

            //mousePositionField.text = $"origin: {ray.origin}\ndirection:{ray.direction}";

            if (Mouse.current.leftButton.isPressed) {
                if (selectedGameObject) {
                    selectedGameObject.transform.position += deltaMousePosition3D;
                }
            } else {
                if (Physics.Raycast(ray, out info, float.PositiveInfinity, cubeLayer)) {
                    selectedGameObject = info.collider.gameObject;
                } else {
                    selectedGameObject = default;
                }
            }
        }
    }
}
