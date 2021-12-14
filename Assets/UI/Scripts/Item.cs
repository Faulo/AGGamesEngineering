using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AGGE.UI {
    public class Item : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

        Canvas mainCanvas;
        Image itemImage;
        RectTransform itemRectTransfrom;
        InventorySlot lastSlot;

        void Start() {
            itemImage = GetComponent<Image>();
            itemRectTransfrom = GetComponent<RectTransform>();
            lastSlot = GetComponentInParent<InventorySlot>();
            if(transform.root.gameObject.TryGetComponent(out Canvas canvas)) {
                mainCanvas = canvas;
            }
        }
        public void OnPointerDown(PointerEventData eventData) {
            transform.SetParent(mainCanvas.transform);
            //transform.SetAsLastSibling();
            itemImage.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData) {
            itemRectTransfrom.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData) {
            itemImage.raycastTarget = true;

            if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out InventorySlot slot)) {   
                transform.SetParent(slot.transform);
                transform.localPosition = Vector3.zero;
                slot.SetItem(this);
                lastSlot.SetItem(null);
                lastSlot = slot;
            } 
            else {
                transform.SetParent(lastSlot.transform);
                transform.localPosition = Vector3.zero;
            }
        }
    }
}