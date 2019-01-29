using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //inputs
    Joystick Axis;
    Button Primary;
    Button Secondary;

    //bool for changing the facing direction. 
    bool isFacingRight;

    private void Start() {
        string[] JoyX = new string[3];
        string[] JoyY = new string[3];
        JoyX[0] = "DPadX";
        Axis = new Joystick(JoyX, JoyY);
    }

    private void CheckFacingDirection() {

    }
}
