using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;
    public Vector2 ScreenSize { get; private set; }
    //specific for different colored backgrounds
    public enum BackgroundColors { blue, yellow }
    public BackgroundColors CurrentBackgroundColor;
    public Color blue;
    public Color yellow;

    private enum CameraStates { stiff, bounded, slide }
    private CameraStates currentState = CameraStates.bounded;

    public Vector2[] Min;
    public Vector2[] Max;
    public float maxDistToPlayer;

    private Bounds[] bounds;

    private int currentBound;

    private void Awake() {
        //there can only be one instance of the camera
        if (instance != null) {
            Debug.LogError("There is already one camera in the scene");
            Destroy(gameObject);
        } else {
            instance = this;
        }

        //this is gotten by all agents and is modified here to have the same number across
        ScreenSize = new Vector2(10f, 10f);
    }

    private void Start() {
        if (CurrentBackgroundColor == BackgroundColors.blue) {
            GetComponent<Camera>().backgroundColor = blue;
        } else if (CurrentBackgroundColor == BackgroundColors.yellow) {
            GetComponent<Camera>().backgroundColor = yellow;
        }

        currentBound = 0;
        
        bounds = new Bounds[Min.Length];
        for (int i = 0; i < Min.Length; i++) {
            bounds[i].SetMinMax(Min[i], Max[i]);
        }
    }

    private void LateUpdate() {
        if (currentState == CameraStates.bounded) {
            Vector3 playerPos = Player.instance.Center;
            Vector3 distanceFromPlayer = playerPos - transform.position;
            Vector3 cameraPos = transform.position;

            if (distanceFromPlayer.x > maxDistToPlayer) {
                cameraPos.x += distanceFromPlayer.x - maxDistToPlayer;
            }
            if (distanceFromPlayer.x < -maxDistToPlayer) {
                cameraPos.x += distanceFromPlayer.x + maxDistToPlayer;
            }
            if (distanceFromPlayer.y > maxDistToPlayer) {
                cameraPos.y += distanceFromPlayer.y - maxDistToPlayer;
            }
            if (distanceFromPlayer.y < -maxDistToPlayer) {
                cameraPos.y += distanceFromPlayer.y + maxDistToPlayer;
            }

            if (currentState == CameraStates.bounded && (bounds[currentBound].min != Vector3.zero && bounds[currentBound].max != Vector3.zero)) {
                cameraPos = bounds[currentBound].ClosestPoint(cameraPos);
            }
            cameraPos.z = -10.0f;
            transform.position = cameraPos;
        }
    }

}
