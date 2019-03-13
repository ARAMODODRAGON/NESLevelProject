using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Entity {
    // this instance of the player (there can only be one)
    public static Player instance;

    //the big and small hitboxes for mario
    [SerializeField] protected BoxCollider2D hitBig;
    [SerializeField] protected BoxCollider2D hitSmall;

    // inputs
    private Joystick axis;
    private Button aBut;
    private Button bBut;

    // the players different states
    public enum Powerups { dead, small, big, fire, leaf, frog };
    public Powerups curPowerUp;
    public int CurrentPow {
        get {
            return (int)curPowerUp;
        }
    }
    private bool isInWater;
    public bool isTransitioning;
    private bool isCrouching;
    private bool isAttack;
    private bool canAttack;

    private Meter XVel;
    private Meter YTime;
    private Meter PMeter;
    public float YVel { get; private set; }

    //animation variables
    private Animator anim;
    private bool hadJumped;

    public GameObject fireBall;

    protected override void Awake() {
        base.Awake();
        curPowerUp = Powerups.small;
        if (instance != null) {
            Debug.LogError("There is already one player in the scene");
            Destroy(gameObject);
        } else {
            instance = this;
        }

        //start small
        IsSmall = true;
        isFacingRight = false;
        Flip();

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
        //end of directional input setup

        XVel = new Meter(maxWalkSpeed);
        YTime = new Meter(0.0f, riseTime);
        YTime.Amount = YTime.Max;
        PMeter = new Meter(0.0f, pTime);
        extraJumptime = 0.0f;

        anim = GetComponent<Animator>();

        isTransitioning = false;
    }

    protected void FixedUpdate() {
        if (!isTransitioning && curPowerUp != Powerups.dead) {

            //do this once in the begining so i dont have to check multiple times
            axis.getDir();
            aBut.getInput();
            bBut.getInput();

            //checks
            CheckForCrouch();
            AttackCheck();
            CheckForFlip();

            if (!isInWater) {
                GroundUpdate();
            } else {
                WaterUpdate();
            }

            //apply the new velocity
            rb.velocity = new Vector3(XVel.Amount, YVel, 0.0f);
        } else if (isTransitioning) {
            rb.velocity = Vector3.zero;
        }

        //updates the properties in the animation controller
        UpdateAnimVariables();
    }

    #region properties and methods

    private bool IsSmall {
        get {
            // if the small collider is enabled and the big collider is disabled
            return hitSmall.enabled && !hitBig.enabled;
        }
        set {
            //if true then enable the small collider
            hitSmall.enabled = value;
            //if true then disable the big collider
            hitBig.enabled = !value;
            //if false then the reverse happens
            if (value) {
                ec.box = hitSmall;
            } else {
                ec.box = hitBig;
            }
        }
    }

    // isFacingRight can be gotten from outside the script
    public bool IsFacingRight() { return isFacingRight; }

    public Vector3 Center {
        get {
            return hitSmall.bounds.center;
        }
    }

    public void CollectItem(string itemName) {
        if (itemName.Equals("Mushroom") && curPowerUp == Powerups.small) {
            IsSmall = true;
            curPowerUp = Powerups.big;
        }
        if (itemName.Equals("FireFlower") && curPowerUp != Powerups.fire) {
            IsSmall = false;
            curPowerUp = Powerups.fire;
        }
    }

    public void TakeDamage(Enemies e) {
        if (YVel < 0.0f) {
            e.TakeDamage("stomp");
            YVel = bounceHeight / riseTime;
            YTime.Amount = YTime.Min;
        } else {
            if (curPowerUp == Powerups.fire || curPowerUp == Powerups.leaf || curPowerUp == Powerups.frog) {
                curPowerUp = Powerups.big;
            } else if (curPowerUp == Powerups.big) {
                curPowerUp = Powerups.small;
            } else {
                curPowerUp = Powerups.dead;
            }
        }
    }

    private void CheckForFlip() {
        //only flips the player if their holding a direction and also is moving in that direction
        //eg. one direction is held but mario is sliding the other way so mario wont flip
        if (isFacingRight && axis.Left && (XVel.Amount <= 0.0f || !ec.IsGrounded)) Flip();
        if (!isFacingRight && axis.Right && (XVel.Amount >= 0.0f || !ec.IsGrounded)) Flip();

        //this is for when they land and they arnt facing the right direction
        if (XVel.Amount < 0.0f && isFacingRight && ec.IsGrounded) Flip();
        if (XVel.Amount > 0.0f && !isFacingRight && ec.IsGrounded) Flip();
    }

    public bool CanEnterPipe(PipeDirection dir) {
        if (dir == PipeDirection.down) {
            return ec.IsGrounded && axis.Down;
        } else if (dir == PipeDirection.left) {
            return ec.IsLeft && axis.Left;
        } else if (dir == PipeDirection.right) {
            return ec.IsRight && axis.Right;
        } else if (dir == PipeDirection.up) {
            return ec.IsCeiling && axis.Up;
        } else {
            return false;
        }
    }

    #endregion

    #region attacking

    private void AttackCheck() {
        if (bBut.ButtonDown && (curPowerUp == Powerups.fire || curPowerUp == Powerups.leaf)) {
            isAttack = true;
        }
        int i = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Fireball")) {
            i++;
        }
        if (i >= 2) {
            canAttack = false;
        } else {
            canAttack = true;
        }

    }

    private void Attack() {
        if (curPowerUp == Powerups.fire && GameObject.FindGameObjectsWithTag("Fireball").Length < 3) {
            Vector3 spawnPos = hitBig.bounds.center;
            spawnPos.y += hitBig.bounds.extents.y / 2.0f;
            GameObject ob = Instantiate(fireBall, spawnPos, Quaternion.Euler(Vector3.zero));
            ob.GetComponent<FireBallController>().IsFacingRight = isFacingRight;
        } else if (curPowerUp == Powerups.leaf) {

        }

    }

    private void EndAttack() {
        isAttack = false;
    }

    #endregion

    private void UpdateAnimVariables() {
        //tell if marios on the ground
        anim.SetBool("IsGrounded", ec.IsGrounded && YVel <= 0.0f);

        //this is for the diffrent run speeds
        if (XVel.Abs >= maxPSpeed) {
            anim.SetInteger("Speed", 4);
        } else if (XVel.Abs >= maxRunSpeed) {
            anim.SetInteger("Speed", 3);
        } else if (XVel.Abs >= maxWalkSpeed) {
            anim.SetInteger("Speed", 2);
        } else if (XVel.Abs > 0.0f) {
            anim.SetInteger("Speed", 1);
        } else {
            anim.SetInteger("Speed", 0);
        }

        if (PMeter.Amount == PMeter.Max) {
            anim.SetBool("PSpeedIsActive", true);
        } else {
            anim.SetBool("PSpeedIsActive", false);
        }

        //this is for jumping
        if (YVel > 0.0f) {
            anim.SetBool("IsRising", true);
        } else {
            anim.SetBool("IsRising", false);
        }

        if (YVel < 0.0f) {
            anim.SetBool("IsFalling", true);
        } else {
            anim.SetBool("IsFalling", false);
        }

        //this is when mario is moving one way and presses the opposite direction
        if (axis.Right && XVel.Amount < 0.0f) {
            anim.SetBool("HeldOppositeDir", true);
        } else if (axis.Left && XVel.Amount > 0.0f) {
            anim.SetBool("HeldOppositeDir", true);
        } else {
            anim.SetBool("HeldOppositeDir", false);
        }

        anim.SetBool("IsCrouching", isCrouching);
        anim.SetInteger("PowerUpState", (int)curPowerUp);
        anim.SetBool("IsAttacking", isAttack);
        anim.SetBool("CanAttack", canAttack);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Water") {
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Water") {
            isInWater = false;
        }
    }
}