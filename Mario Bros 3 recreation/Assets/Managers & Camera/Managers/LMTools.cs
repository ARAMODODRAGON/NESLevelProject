using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMTools : MonoBehaviour {
    public static LMTools inst;

    public static Vector2 ScreenSize = new Vector2(10f, 10f);
    public static Vector2 CamPos {
        get {
            return Camera.position;
        }
    }

    public static Transform Camera;
    private CameraController cc;

    private void Awake() {
        if (!inst) {
            inst = this;
        } else {
            Debug.LogError("There is more than one LMTools script in the scene");
        }

        Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cc = GetComponent<CameraController>();
    }
    
    public void SetCamBounds(string boundName) {
        cc.activeBounds = boundName;
    }
}
