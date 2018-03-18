using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LinkPosByCam : MonoBehaviour {

    public enum LinkType { Left, Right }

    [SerializeField] Camera cam;
    [SerializeField] LinkType linkType;

    float prevPaddind;
	
	// Update is called once per frame
	void Update () {
        if (cam != null) {
            if (!cam.orthographic) {
                Debug.LogError("LinkPosByCam поддерживает только ортаграфический режим камеры. Замените режим и заного привяжите камеру к скрипту.");
                cam = null;
                return;
            }
            float padding = cam.orthographicSize * cam.aspect;
            if (linkType == LinkType.Left) padding = -padding;
            if (padding != prevPaddind) {
                transform.position = new Vector3(padding, transform.position.y, transform.position.z);
                prevPaddind = padding;
            }
        }
	}
}
