using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClockController : MonoBehaviour {

    [SerializeField] Transform hArrow;
    [SerializeField] Transform mArrow;
    [SerializeField] Transform sArrow;
    [SerializeField] double gameTime;
    [SerializeField] UnityEvent stopGame = new UnityEvent();

    System.DateTime newYearTime;
    System.DateTime currentTime;
    bool gameRun;
    
    void Start () {
        newYearTime = new System.DateTime(System.DateTime.Now.Year + 1, 1, 1, 0, 0, 0);
        ResetTimer();
    }
	
	void Update () {
        if (gameRun) {
            currentTime = currentTime.AddSeconds(Time.deltaTime);
            ShowTime(currentTime);
            if (currentTime >= newYearTime) StopGame();
        }
    }

    void ShowTime(System.DateTime time) {
        hArrow.localRotation = Quaternion.Euler(0, 0, 90f - 360f * ((time.Hour < 12 ? time.Hour : time.Hour - 12) / 12f));
        mArrow.localRotation = Quaternion.Euler(0, 0, 90f - 360f * (time.Minute / 60f));
        sArrow.localRotation = Quaternion.Euler(0, 0, 90f - 360f * (time.Second / 60f));
    }

    public void SetGameTime(int seconds) {
        gameTime = seconds;
        ShowTime(newYearTime.AddSeconds(-gameTime));
    }

    public void StartGame() {
        currentTime = newYearTime.AddSeconds(-gameTime);
        gameRun = true;
    }

    public void StopGame() {
        stopGame.Invoke();
        ResetTimer();
    }

    public void ResetTimer() {
        ShowTime(newYearTime.AddSeconds(-gameTime));
        gameRun = false;
    }
}
