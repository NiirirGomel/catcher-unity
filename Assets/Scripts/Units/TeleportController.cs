using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TouchListener), typeof(Rigidbody2D))]
public class TeleportController : MonoBehaviour {

    [SerializeField] float cooldown;

    TouchListener touches;
    Rigidbody2D body;

    float cooldownTimer;
    TouchListener.TouchLast prevTouch;

    void Awake() {
        touches = GetComponent<TouchListener>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        cooldownTimer = cooldown;
        if (body != null) body.velocity = Vector2.zero;
        if (touches != null) prevTouch = touches.GetLastTouch();
    }

    void FixedUpdate () {
        TouchListener.TouchLast lastTouch = touches.GetLastTouch();
        if (cooldownTimer < cooldown) {
            cooldownTimer += Time.fixedDeltaTime;
        } else {
            if (lastTouch != null && lastTouch != prevTouch) {
                body.velocity = Vector2.zero;
                body.position = new Vector2(lastTouch.pos.x, body.position.y);
                cooldownTimer = 0;
            }
        }
        prevTouch = lastTouch;
    }

    public void SetCooldown(float cooldown) {
        this.cooldown = cooldown;
    }
}
