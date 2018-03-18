using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemController))]
public class PresentController : MonoBehaviour {

    public void AddReward(int points) {
        CandyController[] candyObjects = FindObjectsOfType<CandyController>();
        for (int i = 0; i < candyObjects.Length; i++) {
            if (candyObjects[i].gameCandy) candyObjects[i].AddCandy(points);
        }
        if (GameScore.gameScore != null) GameScore.gameScore.CatchItem(GetComponent<ItemController>());
        Destroy(gameObject);
    }

    public void Crash() {
        if (GameScore.gameScore != null) GameScore.gameScore.CrashItem(GetComponent<ItemController>());
        Destroy(gameObject);
    }
}
