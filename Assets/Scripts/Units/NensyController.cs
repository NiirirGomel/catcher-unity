using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NensyController : MonoBehaviour {

    [Header("Настройки таймингов")]
    [SerializeField] float minStayDuration;
    [SerializeField] float maxStayDuration;

    [SerializeField] float minMoveDuration;
    [SerializeField] float maxMoveDuration;

    [Header("Основные параметры")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;

    [Header("Настройки кидания")]
    [SerializeField] List<GameObject> presentPrefs;
    [SerializeField] Gigibody2DTrow trowParams;

    Rigidbody2D body;
    bool gameRun;
    float stayTime;
    float moveTime;

    public enum State { STAY, MOVE, ACTION }
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
                    setState(gameRun ? State.ACTION : State.STAY);
                }
                break;
            case State.ACTION:
                Action();
                setState(State.STAY);
                break;
        }
    }

    void FixedUpdate() {
        switch (state) {
            case State.MOVE: Move(); break;
        }
    }

    void setState(State state) {
        switch (state) {
            case State.STAY:
                stayTime = Random.Range(minStayDuration, maxStayDuration);
                break;
            case State.MOVE:
                direction = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
                moveTime = Random.Range(minMoveDuration, maxMoveDuration);
                break;
        }
        this.state = state;
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

    void Action() {
        if (presentPrefs.Count > 0) {
            Rigidbody2D present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            trowParams.Trow(present);
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

    public void StartGame() {
        gameRun = true;
    }

    public void StopGame() {
        gameRun = false;
    }

    public void AddItem(GameObject go) {
        if (!presentPrefs.Contains(go)) presentPrefs.Add(go);
    }

    public void SetMinStayDuration(float duration) {
        minStayDuration = duration;
    }

    public void SetMaxStayDuration(float duration) {
        maxStayDuration = duration;
    }

    public void SetMinMoveDuration(float duration) {
        minMoveDuration = duration;
    }

    public void SetMaxMoveDuration(float duration) {
        maxMoveDuration = duration;
    }
}
