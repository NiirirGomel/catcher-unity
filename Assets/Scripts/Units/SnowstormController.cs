using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowstormController : MonoBehaviour {
    
    [SerializeField] float stormProgress;
    [SerializeField] float lerpSpeed;

    private void Start() {
        SnowstormItem.lerpSpeed = lerpSpeed;
    }

    public void StartSnowstorm() {
        SnowstormItem.stormProgress = stormProgress;
    }

    public void StopSnowstorm() {
        SnowstormItem.stormProgress = 1f;
    }

    public void SetSnowstormProgress(float progress) {
        stormProgress = progress;
    }
}
