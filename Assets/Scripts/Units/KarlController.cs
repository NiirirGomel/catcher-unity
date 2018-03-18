using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KarlController : MonoBehaviour {

    [Header("Настройки таймингов")]
    [SerializeField] float minStayDuration;
    [SerializeField] float maxStayDuration;

    [SerializeField] float minMoveDuration;
    [SerializeField] float maxMoveDuration;

    [Header("Основные параметры")]
    [SerializeField] Gigibody2DTrow jumpParams;
    [SerializeField] float jumpDelay;
    [SerializeField] float actionDelay;
    [Range(1, 3)] public int comboLevel;

    [Header("Настройки кидания")]
    [SerializeField] List<GameObject> presentPrefs;
    [SerializeField] Gigibody2DTrow FirstTrowParams;
    [SerializeField] Gigibody2DTrow SecondTrowParams;
    [SerializeField] Gigibody2DTrow ThridTrowParams;

    Rigidbody2D body;
    bool gameRun;
    float stayTime;
    float moveTime;
    float actionTime;
    float jumpTime;
    int actionStep;

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
                jumpTime = 0;
                break;
            case State.ACTION:
                actionStep = 1;
                break;
        }
        this.state = state;
    }

    void Move() {
        if ((jumpTime -= Time.deltaTime) <= 0) {
            if (direction == Vector2.right) {
                jumpParams.Trow(body, true);
            } else if (direction == Vector2.left) {
                jumpParams.Trow(body);
            }
            jumpTime = jumpDelay;
        }
    }

    void Action() {
        if (presentPrefs.Count > 0) {
            Rigidbody2D present = null;
            switch (actionStep) {
                case 1:
                    present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position + Vector3.left, Quaternion.identity).GetComponent<Rigidbody2D>();
                    FirstTrowParams.Trow(present);
                    present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position + Vector3.right, Quaternion.identity).GetComponent<Rigidbody2D>();
                    FirstTrowParams.Trow(present, true);
                    actionStep = 2;
                    actionTime = actionDelay;
                    break;
                case 2:
                    if ((actionTime -= Time.deltaTime) <= 0) {
                        present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position + Vector3.left, Quaternion.identity).GetComponent<Rigidbody2D>();
                        SecondTrowParams.Trow(present);
                        present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position + Vector3.right, Quaternion.identity).GetComponent<Rigidbody2D>();
                        SecondTrowParams.Trow(present, true);
                        actionStep = 3;
                        actionTime = actionDelay;
                    }
                    break;
                case 3:
                    if ((actionTime -= Time.deltaTime) <= 0) {
                        present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position + Vector3.left, Quaternion.identity).GetComponent<Rigidbody2D>();
                        ThridTrowParams.Trow(present);
                        present = Instantiate(presentPrefs[Random.Range(0, presentPrefs.Count)].gameObject, transform.position + Vector3.right, Quaternion.identity).GetComponent<Rigidbody2D>();
                        ThridTrowParams.Trow(present, true);
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

    public void AddItem(GameObject go) {
        if (!presentPrefs.Contains(go)) presentPrefs.Add(go);
    }
    
    public void SetComboLevel(int level) {
        comboLevel = level;
    }
}
