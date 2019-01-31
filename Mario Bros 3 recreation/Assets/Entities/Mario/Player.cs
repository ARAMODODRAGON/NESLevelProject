using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    //inputs
    Joystick axis;
    Button primary;
    Button secondary;

    private bool isFacingRight;
    public static bool hasLastMovedRight { get; private set; }

    private void Start() {
        string[] JoyX = new string[3];
        string[] JoyY = new string[3];
        JoyX[0] = "DPadX";
        axis = new Joystick(JoyX, JoyY);

        isFacingRight = true;
        hasLastMovedRight = true;
    }

    private void CheckFacingDirection() {

    }

}
