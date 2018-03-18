using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StiveController : MonoBehaviour {

    [Header("Настройки таймингов")]
    [SerializeField] float minStayDuration;
    [SerializeField] float maxStayDuration;

    [SerializeField] float minMoveDuration;
    [SerializeField] float maxMoveDuration;

    [Header("Основные параметры")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float jumpForce;
    [SerializeField] Transform santaTransform;
    [SerializeField] float actionDelay;
    [Range(1, 3)] public int comboLevel;

    [Header("Настройки кидания")]
    [SerializeField] GameObject[] presentPrefs;
    [SerializeField] Gigibody2DTrow FirstTrowParams;
    [SerializeField] Gigibody2DTrow SecondTrowParams;
    [SerializeField] Gigibody2DTrow ThridTrowParams;



    Rigidbody2D body;
    bool gameRun;
    float stayTime;
    float moveTime;
    float actionTime;
    int actionStep;

    public enum State { STAY, MOVE, ACTION }
    State state;
    Vector2 direction = Vector2.right;

    // Use this for initialization
    void Start () {
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
                if (gameRun && actionStep <= comboLevel) {
                    Action();
                } else {
                    setState(State.STAY);
                }
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
            case State.ACTION:
                actionStep = 1;
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
        if (presentPrefs.Length > 0) {
            Rigidbody2D present = null;
            switch (actionStep) {
                case 1:
                    present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Length)].gameObject, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                    FirstTrowParams.Trow(present, direction == Vector2.right ? true : false);
                    actionStep = 2;
                    actionTime = actionDelay;
                    break;
                case 2:
                    if ((actionTime -= Time.deltaTime) <= 0) {
                        present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Length)].gameObject, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                        SecondTrowParams.Trow(present, direction == Vector2.left ? true : false);
                        if (comboLevel > 2) body.AddForce(Vector2.up * jumpForce);
                        actionStep = 3;
                        actionTime = actionDelay;
                    }
                    break;
                case 3:
                    if ((actionTime -= Time.deltaTime) <= 0) {
                        if (santaTransform != null) direction = transform.position.x - santaTransform.position.x > 0 ? Vector2.left : Vector2.right;
                        present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Length)].gameObject, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                        ThridTrowParams.Trow(present, direction == Vector2.right ? true : false);
                        actionStep = comboLevel + 1;
                    }
                    break;
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

    public void StartGame() {
        gameRun = true;
    }

    public void StopGame() {
        gameRun = false;
    }

    public void SetComboLevel(int level) {
        comboLevel = level;
    }

    public void SetSantaTransform(Transform santa) {
        santaTransform = santa;
    }
}
