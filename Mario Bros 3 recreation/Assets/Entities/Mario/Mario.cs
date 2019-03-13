using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mario : Entity {

    [Header("Colliders")]
    [SerializeField]
    private BoxCollider2D bigCollider;
    [SerializeField]
    private BoxCollider2D smallCollider;

    //vel variables
    private Vector2 vel;
    private float MaxXVel;
    private float MaxYVel;

    //states
    private enum PowerUps {
        Dead,
        Small,
        Big,
        Fire,
        Leaf,
        Frog
    };
    private PowerUps curPowerUp = PowerUps.Small;
    [HideInInspector]
    public bool isTransitioning;
    public bool isInWater;

    // inputs
    private Joystick axis;
    private Button aBut;
    private Button bBut;

    //properties
    public bool IsSmall {
        get {
            // if the small collider is enabled and the big collider is disabled
            return smallCollider.enabled && !bigCollider.enabled;
        }
        set {
            //if true then enable the small collider
            smallCollider.enabled = value;
            //if true then disable the big collider
            bigCollider.enabled = !value;
            //if false then the reverse happens
            if (value) {
                ec.box = smallCollider;
            } else {
                ec.box = bigCollider;
            }
        }
    }

    public bool IsFacingRight() { return isFacingRight; }

    protected override void Awake() {
        base.Awake();

        #region input setup
        //start setting up the directional inputs and button inputs
        string[] JoyX = new string[3];
        string[] JoyY = new string[3];
        JoyX[0] = "DPadX";
        JoyX[1] = "JoyX";
        JoyX[2] = "ArrowkeysX";
        JoyY[0] = "DPadY";
        JoyY[1] = "JoyY";
        JoyY[2] = "ArrowkeysY";
        axis = new Joystick(JoyX, JoyY);

        string[] ButtonA = new string[2];
        ButtonA[0] = "Controller A";
        ButtonA[1] = "Keyboard A";
        aBut = new Button(ButtonA);

        string[] ButtonB = new string[2];
        ButtonB[0] = "Controller B";
        ButtonB[1] = "Keyboard B";
        bBut = new Button(ButtonB);
        #endregion

    }

    private void FixedUpdate() {
        //update inputs
        axis.getDir();
        aBut.getInput();
        bBut.getInput();

        if (isTransitioning) {
            rb.velocity = Vector2.zero;
        } else {
            vel = rb.velocity;

            if (isInWater) {
                FixedWaterUpdate();
            } else {
                FixedGroundUpdate();
            }

            rb.velocity = vel;
        }
    }
}
