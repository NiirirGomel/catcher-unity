using UnityEngine;
using System.Collections;

[System.Serializable]
public class Gigibody2DTrow {
    public float force;
    public float angleMin;
    public float angleMax;
    public float rotMin;
    public float rotMax;

    public void Trow(Rigidbody2D body, bool inverseDirection = false) {
        Quaternion angle = Quaternion.Slerp(Quaternion.Euler(0, 0, angleMin), Quaternion.Euler(0, 0, angleMax), Random.value);
        if (inverseDirection) angle = Quaternion.Inverse(angle);
        body.AddForce(angle * Vector2.up * force);
        body.AddTorque(Random.Range(rotMin, rotMax));
    }
}
