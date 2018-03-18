using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SnowstormItem : MonoBehaviour {
    
    public static float stormProgress = 1f;
    public static float lerpSpeed = 1f;

    Rigidbody2D body;
    float lerpProgress = 1f;
    Vector3 prevPos;
    Vector2 prevVelocity;
    float prevAngVelocity;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        prevPos = body.position;
        prevVelocity = body.velocity;
    }

    // Update is called once per frame
    void Update () {
        if (lerpProgress != stormProgress) {
            float duration = Time.deltaTime * lerpSpeed;
            if (lerpProgress < stormProgress) {
                lerpProgress = lerpProgress + duration < stormProgress ? lerpProgress + duration : stormProgress;
            } else {
                lerpProgress = lerpProgress - duration > stormProgress ? lerpProgress - duration : stormProgress;
            }
        }
        if (lerpProgress != 1f) {
            transform.position += (transform.position - prevPos) * (lerpProgress - 1f);
            body.velocity += (body.velocity - prevVelocity) * (lerpProgress - 1f);
            body.angularVelocity += (body.angularVelocity - prevAngVelocity) * (lerpProgress - 1f);
        }
        prevPos = body.position;
        prevVelocity = body.velocity;
        prevAngVelocity = body.angularVelocity;
    }
}
