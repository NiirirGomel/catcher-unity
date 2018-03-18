using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemController : MonoBehaviour {

    public UnityEvent onGround;
    public UnityEvent onSantaHands;

    float m_lifeTime;
    public float lifeTime { get { return m_lifeTime; } }


    // Update is called once per frame
    void Update() {
        m_lifeTime += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Ground") {
            onGround.Invoke();
        }
    }

    void OnTriggerStay2D(Collider2D collision) {
        SantaHands santaHands = collision.gameObject.GetComponent<SantaHands>();
        if (santaHands != null && m_lifeTime > santaHands.ignoreLessLifeTime) {
            onSantaHands.Invoke();
        }
    }
}
