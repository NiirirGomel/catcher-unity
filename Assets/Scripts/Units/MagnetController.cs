using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour {

    [SerializeField] float magnetPower;
    [SerializeField] SantaHands santaHands;
    [SerializeField] CircleCollider2D circleCollider;

    Rigidbody2D body;
    
    void Start () {
        body = GetComponent<Rigidbody2D>();
    }
	
	void Update () {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(body.position, circleCollider.radius * Mathf.Max(circleCollider.transform.lossyScale.x, circleCollider.transform.lossyScale.y), 1 << LayerMask.NameToLayer("Missile"));
        for (int i = 0; i < colliders.Length; i++) {
            ItemController item = colliders[i].GetComponent<ItemController>();
            if (item != null && item.GetComponent<ItemController>().lifeTime > santaHands.ignoreLessLifeTime) {
                Rigidbody2D missileBody = item.GetComponent<Rigidbody2D>();
                if (missileBody != null && item.GetComponent<BoombController>() == null) {
                    Vector2 magnetForse = body.position - missileBody.position;
                    missileBody.AddForce(magnetForse.normalized * magnetPower * Time.fixedDeltaTime);
                }
                SnowstormItem stormEffect = missileBody.GetComponent<SnowstormItem>();
                if (stormEffect != null && stormEffect.enabled) {
                    stormEffect.enabled = false;
                }
            }
        }
    }

    public void SetMagnetPower(float power) {
        magnetPower = power;
    }
}
