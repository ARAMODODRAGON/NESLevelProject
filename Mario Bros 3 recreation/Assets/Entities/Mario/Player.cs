using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // this instance of the player
    public static Player instance;

    // inputs
    Joystick axis;
    Button primary;
    Button secondary;

    // isFacingRight can be gotten from outside the script but only modified here
    public bool IsFacingRight {
        get {
            return isFacingRight;
        }
        private set {
            isFacingRight = value;
        }
    }

    protected override void Start() {
        if (instance != null) {
            Debug.LogError("There is already one player in the scene");
            Destroy(gameObject);
        } else {
            instance = this;
        }
        base.Start();
        string[] JoyX = new string[3];
        string[] JoyY = new string[3];
        JoyX[0] = "DPadX";
        axis = new Joystick(JoyX, JoyY);
        
    }

    protected void FixedUpdate() {
        Vector2 axis = Vector2.zero;
        axis.x = Input.GetAxis("ArrowkeysX");
        rb.AddForce(axis * 10.0f);
        if (Input.GetKeyDown(KeyCode.C)) {
            rb.AddRelativeForce(new Vector2(0.0f, 10.0f), ForceMode2D.Impulse);
        }
    }
}
