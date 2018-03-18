using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LuisController : MonoBehaviour {

    [Header("Настройки таймингов")]
    [SerializeField] float minStayDuration;
    [SerializeField] float maxStayDuration;

    [SerializeField] float minMoveDuration;
    [SerializeField] float maxMoveDuration;

    [Header("Основные параметры")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float stunDuration;
    [SerializeField] float ignoreLessLifeTime;
    [SerializeField] BoxCollider2D hands;
    [SerializeField] CircleCollider2D viewCircle;

    [Header("Дроп предметов")]
    [SerializeField] List<Transform> itemPoints;
    [SerializeField] Gigibody2DTrow[] dropParams;

    Rigidbody2D body;
    bool gameRun;
    float stayTime;
    float moveTime;
    float stunTime;

    public enum State { STAY, MOVE, STUN }
    State state;
    Vector2 direction = Vector2.right;

    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody2D>();
        setState(State.STAY);
    }


    void Update() {
        switch (state) {
            case State.STAY:
                if ((stayTime -= Time.deltaTime) <= 0) setState(State.MOVE);
                break;
            case State.MOVE:
                if ((moveTime -= Time.deltaTime) <= 0) {
                    setState(State.STAY);
                }
                break;
            case State.STUN:
                if ((stunTime -= Time.deltaTime) <= 0) setState(State.STAY);
                break;
        }
    }

    void FixedUpdate() {
        switch (state) {
            case State.MOVE: Move(); break;
        }
        if (gameRun && state != State.STUN) CatchMissile();
    }

    void setState(State state) {
        switch (state) {
            case State.STAY:
                stayTime = Random.Range(minStayDuration, maxStayDuration);
                break;
            case State.MOVE:
                direction = GetDirToCloseMissile();
                moveTime = Random.Range(minMoveDuration, maxMoveDuration);
                break;
            case State.STUN:
                stunTime = stunDuration;
                break;
        }
        this.state = state;
    }

    Vector2 GetDirToCloseMissile() {
        Transform closeMissile = null;
        Collider2D[] missiles = Physics2D.OverlapCircleAll(GetComponent<Rigidbody2D>().position, viewCircle.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), 1 << LayerMask.NameToLayer("Missile"));
        for (int i = 0; i < missiles.Length; i++) {
            if (missiles[i].GetComponent<BoombController>() == null) {
                if (closeMissile == null || Vector2.Distance(missiles[i].transform.position, transform.position) < Vector2.Distance(closeMissile.position, transform.position)) {
                    closeMissile = missiles[i].transform;
                }
            }
        }
        if (closeMissile != null) {
            return closeMissile.position.x - transform.position.x < 0 ? Vector2.left : Vector2.right;
        } else {
            return Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        }
    }

    void Move() {
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

    void CatchMissile() {
        if (itemPoints != null) {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(body.position + hands.offset, new Vector2(hands.size.x * transform.lossyScale.x, hands.size.y * transform.lossyScale.y), 0, 1 << LayerMask.NameToLayer("Missile"));
            for (int i = 0; i < colliders.Length; i++) {
                Transform point = GetFreePoint();
                if (point != null) {
                    ItemController item = colliders[i].GetComponent<ItemController>();
                    if (item != null && item.GetComponent<BoombController>() == null && item.lifeTime > ignoreLessLifeTime && !ContainsItem(item)) {
                        AttachItemToPoint(point, item);
                    }
                }
            }
        }
    }

    Transform GetFreePoint() {
        for (int i = 0; i < itemPoints.Count; i++) {
            if (null == itemPoints[i].GetComponentInChildren<ItemController>()) return itemPoints[i];
        }
        return null;
    }

    bool ContainsItem(ItemController item) {
        for (int i = 0; i < itemPoints.Count; i++) {
            if (item == itemPoints[i].GetComponentInChildren<ItemController>()) return true;
        }
        return false;
    }

    void AttachItemToPoint(Transform point, ItemController item) {
        SnowstormItem stormEffect = item.GetComponent<SnowstormItem>();
        if (stormEffect != null) stormEffect.enabled = false;
        Rigidbody2D body = item.GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.isKinematic = true;
        item.gameObject.layer = LayerMask.NameToLayer("DefaultOnly");
        item.GetComponent<SpriteRenderer>().sortingOrder = 2;
        item.transform.SetParent(point);
        item.transform.localPosition = Vector2.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    void DropItems() {
        if (itemPoints != null) {
            for (int i = 0; i < itemPoints.Count; i++) {
                ItemController item = itemPoints[i].GetComponentInChildren<ItemController>();
                if (item != null) {
                    item.transform.SetParent(null);
                    Rigidbody2D body = item.GetComponent<Rigidbody2D>();
                    if (body != null) {
                        body.isKinematic = false;
                        if (dropParams != null && i < dropParams.Length) dropParams[i].Trow(body);
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (state == State.MOVE) {
            if (collision.gameObject.name == "Left Border") {
                direction = Vector2.right;
            } else if (collision.gameObject.name == "Right Border") {
                direction = Vector2.left;
            }
        }
    }

    public void Stun() {
        DropItems();
        setState(State.STUN);
    }

    public void StartGame() {
        gameRun = true;
    }

    public void StopGame() {
        gameRun = false;
        DropItems();
    }

    public void AddItemPoint(Transform transform) {
        if (!itemPoints.Contains(transform)) itemPoints.Add(transform);
    }

    public void SetStunDuration(float duration) {
        stunDuration = duration;
    }
}
