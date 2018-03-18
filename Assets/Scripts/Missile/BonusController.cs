using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemController))]
public class BonusController : MonoBehaviour {

    public void StartBonus(string bonusName) {
        Bonus[] bonuses = Resources.FindObjectsOfTypeAll<Bonus>();
        for (int i = 0; i < bonuses.Length; i++) {
            if (bonuses[i].name == bonusName) bonuses[i].StartBonus();
        }
        if (GameScore.gameScore != null) GameScore.gameScore.CatchItem(GetComponent<ItemController>());
        Destroy(gameObject);
    }

    public void Crash() {
        if (GameScore.gameScore != null) GameScore.gameScore.CrashItem(GetComponent<ItemController>());
        Destroy(gameObject);
    }
}
