using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Camera Bounds

    //enum used in part with the class
    public enum BoundDirection { Horizontal, Vertical, DoubleHori }

    //class used for creating camera bounds
    [System.Serializable]
    public class CameraBounds {
        public string name;
        public BoundDirection direction = BoundDirection.Horizontal;
        public float min;
        public float max;
        public float axisAlignment;
    }

    #endregion
    public List<CameraBounds> allBounds = new List<CameraBounds>();

    //specific for different colored backgrounds
    public enum BackgroundColors { blue, yellow, black }
    public BackgroundColors CurrentBG;
    #region Predefined Colors
    private readonly Color blue = new Color(0.6f, 0.9254f, 0.9686f);
    private readonly Color yellow = new Color(0.996f, 0.8078f, 0.5137f);
    private readonly Color black = new Color(0f, 0f, 0f);
    #endregion

    private readonly float maxDistToPlayer = 1f;
    public string activeBounds;
    
    private void Start() {
        #region set BG color
        if (CurrentBG == BackgroundColors.blue) {
            LMTools.Camera.GetComponent<Camera>().backgroundColor = blue;
        } else if (CurrentBG == BackgroundColors.yellow) {
            LMTools.Camera.GetComponent<Camera>().backgroundColor = yellow;
        } else if (CurrentBG == BackgroundColors.black) {
            LMTools.Camera.GetComponent<Camera>().backgroundColor = black;
        }
        #endregion
    }

    private void LateUpdate() {
        if (allBounds.Count == 0) {
            return;
        }

        Vector3 playerPos = Player.instance.Center;
        Vector3 distanceFromPlayer = playerPos - transform.position;
        Vector3 cameraPos = transform.position;
        CameraBounds cb = allBounds.Find(CameraBounds => CameraBounds.name == activeBounds);
        if (cb == null) {
            Debug.LogError("Could not find bounds of name: " + activeBounds);
            return;
        }

        //moves the camera towards the player according to how far they are
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

        //checks if it should align horizontally or vertically
        if (cb.direction == BoundDirection.Horizontal) {

            cameraPos.y = cb.axisAlignment;
            if (cameraPos.x < cb.min) cameraPos.x = cb.min;
            if (cameraPos.x > cb.max) cameraPos.x = cb.max;

        } else if (cb.direction == BoundDirection.Vertical) {

            cameraPos.x = cb.axisAlignment;
            if (cameraPos.y < cb.min) cameraPos.y = cb.min;
            if (cameraPos.y > cb.max) cameraPos.y = cb.max;
        }

        cameraPos.z = -10f;
        LMTools.Camera.position = cameraPos;

    }


}
