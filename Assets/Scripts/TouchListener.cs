using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchListener : MonoBehaviour {

    [SerializeField] bool debug;

    LinkedList<TouchLast> touchList = new LinkedList<TouchLast>();

    // Update is called once per frame
    void Update() {
        ListenTouches();                // сортировка сасаний экрана
        ListenSimulationTouches();      // симуляция касаний для дебага в едиторе
    }

    void OnGUI() {
        if (debug) TouchDataDebug();    // дебаг касаний
    }

    // сортировка сасаний экрана
    void ListenTouches() {
        Touch[] touches = Input.touches;
        for (int i = 0; i < touches.Length; i++) {
            switch (touches[i].phase) {
                case TouchPhase.Began: {
                        touchList.AddLast(new TouchLast(touches[i].fingerId, Camera.main.ScreenToWorldPoint(touches[i].position)));
                        break;
                    }
                case TouchPhase.Moved: {
                        for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
                            if (node.Value.fingerId == touches[i].fingerId) {
                                node.Value.pos = Camera.main.ScreenToWorldPoint(touches[i].position);
                                break;
                            }
                        }
                        break;
                    }
                case TouchPhase.Canceled:
                case TouchPhase.Ended: {
                        for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
                            if (node.Value.fingerId == touches[i].fingerId) {
                                touchList.Remove(node);
                                break;
                            }
                        }
                        break;
                    }
            }
        }
    }

    // симуляция касаний для дебага в едиторе
    void ListenSimulationTouches() {
        if (Input.GetKeyDown(KeyCode.A)) {
            touchList.AddLast(new TouchLast(0, Vector2.left));
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
                if (node.Value.fingerId == 0) {
                    touchList.Remove(node);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            touchList.AddLast(new TouchLast(1, Vector2.right));
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
                if (node.Value.fingerId == 1) {
                    touchList.Remove(node);
                    break;
                }
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            touchList.AddLast(new TouchLast(3, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
        if (Input.GetMouseButtonUp(0)) {
            for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
                if (node.Value.fingerId == 3) {
                    touchList.Remove(node);
                    break;
                }
            }
        }
        if (Input.GetMouseButton(0)) {
            for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
                if (node.Value.fingerId == 3) {
                    node.Value.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    break;
                }
            }
        }
    }

    // дебаг касаний
    void TouchDataDebug() {
        string text = "";
        for (LinkedListNode<TouchLast> node = touchList.First; node != null; node = node.Next) {
            text += "fingerId: " + node.Value.fingerId + "\n";
            text += "pos: " + node.Value.pos.ToString() + "\n";
        }
        GUIStyle debugStyle = GUI.skin.GetStyle("TextArea");
        debugStyle.fontSize = 27;
        if (text.Length > 0) GUILayout.TextField(text, debugStyle);
    }
    
    // позиция последнего нажатого поинтера
    public TouchLast GetLastTouch() {
        if (touchList.Last != null) return touchList.Last.Value;
        return null;
    }

    // класс для сортировки касаний
    public class TouchLast {
        public int fingerId;
        public Vector2 pos;
        public TouchLast(int fingerId, Vector2 pos) {
            this.fingerId = fingerId;
            this.pos = pos;
        }
    }
}
