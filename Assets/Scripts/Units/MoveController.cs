using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TouchListener), typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour {

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;

    TouchListener touches;
    Rigidbody2D body;

    Vector2 direction = Vector2.right;

    void Start() {
        touches = GetComponent<TouchListener>();
        body = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    void FixedUpdate() {
        direction = TouchDirection(touches.GetLastTouch());
        if (direction == Vector2.right) {
            if (body.velocity.x < maxSpeed) {
                if ((body.velocity += direction * acceleration * Time.fixedDeltaTime).x > maxSpeed) {
                    body.velocity = new Vector2(maxSpeed, body.velocity.y);
                }
            }
        } else if (direction == Vector2.left) {
            if (body.velocity.x > -maxSpeed) {
                if ((body.velocity += direction * acceleration * Time.fixedDeltaTime).x < -maxSpeed) {
                    body.velocity = new Vector2(-maxSpeed, body.velocity.y);
                }
            }
        }
    }

    Vector2 TouchDirection(TouchListener.TouchLast touch) {
        if (touch != null) {
            return touch.pos.x - Camera.main.transform.position.x > 0 ? Vector2.right : Vector2.left;
        } else {
            return Vector2.zero;
        }
    }

    public void SetMaxSpeed(float speed) {
        maxSpeed = speed;
    }

    public void SetAcceleration(float acceleration) {
        this.acceleration = acceleration;
    }
}