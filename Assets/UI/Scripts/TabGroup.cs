using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour {
    [SerializeField]
    List<TabButton> tabButtons = default;
    [SerializeField]
    List<GameObject> objectsToSwap = default;
    [SerializeField]
    Sprite tabIdleSprite = default;
    [SerializeField]
    Sprite tabHoverSprite = default;
    [SerializeField]
    Sprite tabActiveSprite = default;
    [SerializeField]
    Color tabActiveFontColor = default;
    [SerializeField]
    Color tabIdleFontColor = default;

    TabButton selectedTab;

    protected void Start() {
        OnTabSelected(tabButtons.First());
        ResetTabs();
    }
    public void NavigateToTheLeft() {
        int index = tabButtons.IndexOf(selectedTab);
        if (index == 0) {
            OnTabSelected(tabButtons.Last());
        } else {
            OnTabSelected(tabButtons[index - 1]);
        }
    }
    public void NavigateToTheRight() {
        int index = tabButtons.IndexOf(selectedTab);
        if (index == tabButtons.Count - 1) {
            OnTabSelected(tabButtons.First());
        } else {
            OnTabSelected(tabButtons[index + 1]);
        }
    }

    public void OnTabEnter(TabButton button) {
        Assert.IsTrue(button);
        ResetTabs();
        if (selectedTab == null || button != selectedTab) {
            button.background.sprite = tabHoverSprite;
        }
    }

    public void OnTabExit(TabButton button) {
        Assert.IsTrue(button);
        ResetTabs();
    }

    public void OnTabSelected(TabButton button) {
        Assert.IsTrue(button);
        if (selectedTab) {
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.background.sprite = tabActiveSprite;
        button.text.color = tabActiveFontColor;

        int index = tabButtons.IndexOf(button);
        for (int i = 0; i < objectsToSwap.Count; i++) {
            objectsToSwap[i].SetActive(i == index);
        }

        //if (objectsToSwap[index].transform.TryGetComponentInChildren<Selectable>(out var selectable)) {
        //    observedComponent.currentSelectedGameObject = selectable.gameObject;
        //}
    }

    void ResetTabs() {
        foreach (var button in tabButtons) {
            if (button == selectedTab) {
                continue;
            }
            button.background.sprite = tabIdleSprite;
            button.text.color = tabIdleFontColor;
        }
    }
}