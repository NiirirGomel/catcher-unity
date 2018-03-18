using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Bonus : MonoBehaviour {

    [SerializeField] Text timerText;
    [SerializeField] float bonusTime;
    [SerializeField] UnityEvent startBonus;
    [SerializeField] UnityEvent endBonus;
    
    float timer;

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf) {
            timer += Time.deltaTime;
            timerText.text = ((int)(bonusTime - timer + 1)).ToString();
            if (timer >= bonusTime) {
                EndBonus();
            }
        }
	}

    public void StartBonus() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
            Transform myParent = transform.parent;
            transform.SetParent(null);
            transform.SetParent(myParent);
            startBonus.Invoke();
        }
        timer = 0;
    }

    public void EndBonus() {
        gameObject.SetActive(false);
        endBonus.Invoke();
    }

    public void SetBonusTime(float time) {
        bonusTime = time;
    }
}
