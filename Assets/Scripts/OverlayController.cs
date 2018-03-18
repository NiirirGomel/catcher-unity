using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlayController : MonoBehaviour {

    [SerializeField] EscapeObject[] escapeObjects;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            for (int i = 0; i < escapeObjects.Length; i++) {
                if (escapeObjects[i].escapeObject.activeSelf) {
                    escapeObjects[i].escapeEvent.Invoke();
                    break;
                }
            }
        }
    }

    public void SetTimeScale(float scale) {
        Time.timeScale = scale;
    }

    public void ExitGame() {
        Application.Quit();
    }

    [System.Serializable]
    struct EscapeObject {
        public GameObject escapeObject;
        public UnityEvent escapeEvent;
    }
}
