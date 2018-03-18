using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfo : MonoBehaviour {

    [SerializeField] CandyController candyController;
    [SerializeField] Text title;
    [SerializeField] Text description;
    [SerializeField] Button updradeButton;
    [SerializeField] Text cost;

    UpgradeController focusItem;

    public void SetFocus(UpgradeController item) {
        if (item.nextLevel < item.levelData.Length) {
            if (focusItem != null) focusItem.transform.localScale = Vector3.one;
            focusItem = item;
            focusItem.transform.localScale = Vector3.one * 1.2f;
            title.text = focusItem.title;
            description.text = focusItem.levelData[focusItem.nextLevel].description;

            updradeButton.onClick.RemoveAllListeners();
            if (focusItem.nextLevel < item.levelData.Length - 1) {
                cost.gameObject.SetActive(true);
                updradeButton.gameObject.SetActive(true);
                updradeButton.GetComponentInChildren<Text>().text = focusItem.nextLevel == 0 ? "BUY" : "UPGRADE";
                updradeButton.onClick.AddListener(focusItem.UpgradeEvent);
                updradeButton.interactable = candyController.candyCount >= focusItem.levelData[focusItem.nextLevel].cost ? true : false;
                cost.text = focusItem.levelData[focusItem.nextLevel].cost.ToString();
            } else {
                cost.gameObject.SetActive(false);
                updradeButton.gameObject.SetActive(false);
            }
        }
    }
}
