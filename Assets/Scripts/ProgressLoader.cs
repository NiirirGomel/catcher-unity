using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLoader : MonoBehaviour {

    [SerializeField] CandyController loadCandy;
    [SerializeField] UpgradeController[] loadUpgrades;
    
    void Start () {
        if (loadCandy != null) loadCandy.Load();
        if (loadUpgrades != null) for (int i = 0; i < loadUpgrades.Length; i++) loadUpgrades[i].Load();
	}
}
