using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefuseController : MonoBehaviour {

    [SerializeField] float defusePower = 1f;
    [SerializeField] SantaHands santaHands;
    [SerializeField] CircleCollider2D circleCollider;

    Rigidbody2D body;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(body.position, circleCollider.radius * Mathf.Max(circleCollider.transform.lossyScale.x, circleCollider.transform.lossyScale.y), 1 << LayerMask.NameToLayer("Missile"));
        for (int i = 0; i < colliders.Length; i++) {
            BoombController boomb = colliders[i].GetComponent<BoombController>();
            if (boomb != null && boomb.GetComponent<ItemController>().lifeTime > santaHands.ignoreLessLifeTime) {
                boomb.Defuse(Time.fixedDeltaTime * defusePower);
            }
        }
    }

    public void SetDefusePower(float power) {
        defusePower = power;
    }
}
