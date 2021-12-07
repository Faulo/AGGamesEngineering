using UnityEngine;

public class InventoryTab : MonoBehaviour {
    [SerializeField]
    GameObject contentPanel = default;

    public void OpenPanel() {
        contentPanel.SetActive(true);
    }

    public void ClosePanel() {
        contentPanel.SetActive(false);
    }
}
