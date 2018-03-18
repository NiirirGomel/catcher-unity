using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemController))]
public class BoombController : MonoBehaviour {
    
    [SerializeField] CircleCollider2D explosionCircle;
    [SerializeField] float explosionPower;
    [SerializeField] int damage;
    [SerializeField] LayerMask explosionMask;
    [SerializeField] float defuseDuration;

    float defuseProgress;

    public void Explosion() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GetComponent<Rigidbody2D>().position, explosionCircle.radius * Mathf.Max(explosionCircle.transform.lossyScale.x, explosionCircle.transform.lossyScale.y), explosionMask);
        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody2D body = colliders[i].GetComponent<Rigidbody2D>();
            if (body != null) {
                Vector2 direction = body.position - GetComponent<Rigidbody2D>().position;
                body.AddForce(direction.normalized * explosionPower, ForceMode2D.Impulse);
            }
        }
        Collider2D[] units = Physics2D.OverlapCircleAll(GetComponent<Rigidbody2D>().position, explosionCircle.radius * Mathf.Max(explosionCircle.transform.lossyScale.x, explosionCircle.transform.lossyScale.y), 1 << LayerMask.NameToLayer("Units") | 1 << LayerMask.NameToLayer("Santa"));
        for (int i = 0; i < units.Length; i++) {
            LuisController luis = units[i].GetComponent<LuisController>();
            if (luis != null) luis.Stun();
            DamageController santa = units[i].GetComponent<DamageController>();
            if (santa != null) {
                santa.TakeDamage(damage);
                if (GameScore.gameScore != null) GameScore.gameScore.CatchItem(GetComponent<ItemController>());
            } else {
                if (GameScore.gameScore != null) GameScore.gameScore.CrashItem(GetComponent<ItemController>());
            }
        }
        Destroy(gameObject);
    }

    public void Defuse(float progress) {
        defuseProgress += progress;
        if (defuseProgress >= defuseDuration) {
            Destroy(gameObject);
        } else {
            // визуализация прогресса разминирования
            GetComponent<Renderer>().material.color = new Color(1f, 1f - defuseProgress / defuseDuration, 1f - defuseProgress / defuseDuration);
        }
    }
}
