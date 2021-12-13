using UnityEngine;

namespace AGGE.UI {
    public class InventorySlot : MonoBehaviour {
        [SerializeField]
        Item item = default;

        Item currentItem = default;
        Item currentItemInstance = default;

        protected void Start() {
            if (item) {
                var itemInstance = Instantiate(item);
                itemInstance.gameObject.transform.SetParent(transform);
                itemInstance.transform.localPosition = Vector3.zero;
                currentItem = item;
                currentItemInstance = itemInstance;
            }
        }

        protected void Update() {
            //if (item && item != currentItem) {
            //    Destroy(currentItemInstance);
            //    var itemInstance = Instantiate(item);
            //    itemInstance.gameObject.transform.SetParent(transform);
            //    itemInstance.GetComponent<RectTransform>().sizeDelta = Vector2.one;
            //    itemInstance.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            //    itemInstance.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            //    currentItem = item;
            //    currentItemInstance = itemInstance;
            //}
        }
        public void SetItem(Item item) {
            currentItem = item;
            currentItemInstance = item;
        }
    }
}