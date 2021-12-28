using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AGGE.UI {
    [RequireComponent(typeof(Image))]
    public class TabMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
        [HideInInspector]
        public Image background = default;

        [SerializeField]
        TabMenuGroup tabGroup = default;
        [SerializeField]
        public TextMeshProUGUI text = default;

        [SerializeField]
        UnityEvent onTabSelected = default;
        [SerializeField]
        UnityEvent onTabDeselected = default;

        protected void Awake() {
            SetUpComponents();
        }
        protected void Start() {
            SetUpComponents();
        }
        protected void SetUpComponents() {
            if (!background) {
                TryGetComponent(out background);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            tabGroup.OnTabExit(this);
        }

        public void Select() {
            if (onTabSelected != null) {
                onTabSelected.Invoke();
            }
        }

        public void Deselect() {
            if (onTabDeselected != null) {
                onTabDeselected.Invoke();
            }
        }
    }
}