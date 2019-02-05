using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    // this instance of the player (there can only be one)
    public static Player instance;

    //the big and small hitboxes for mario
    [SerializeField] protected Collider2D trigBig;
    [SerializeField] protected Collider2D trigSmall;
    [SerializeField] protected Collider2D hitBig;
    [SerializeField] protected Collider2D hitSmall;

    // inputs
    private Joystick axis;
    private Button aBut;
    private Button bBut;

    // the players different states
    private enum powerups {dead, small, big, fire, leaf, frog};
    private powerups curPowerUp;
    private bool isJumping;
    private bool isCrouching;
    private bool isAttack;
    private bool isHover;

    //movement variables
    public float maxWalkSpeed;
    public float maxRunSpeed;
    public float maxPSpeed;
    public float pTime;
    public float XAccel;
    public float XDeccel;

    public float jumpHeight;
    public float riseTime;
    public float fallSpeed;
    public float fallAccel;
    public float minJumpHeight;
    public float extraJumpHeight;
    private float extraJumptime;

    private Meter XVel;
    private Meter YTime;
    private Meter PMeter;
    private float YVel;

    //animation variables
    private Animator anim;
    private bool hadJumped;

    public GameObject fireBall;

    //=============================================================================================================================================//

    protected override void Start() {
        if (instance != null) {
            Debug.LogError("There is already one player in the scene");
            Destroy(gameObject);
        } else {
            instance = this;
        }
        base.Start();
        //start small
        IsSmall = false; 
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

        curPowerUp = powerups.small;
        XVel = new Meter(maxWalkSpeed);
        YTime = new Meter(0.0f, riseTime);
        YTime.Amount = YTime.Max;
        PMeter = new Meter(0.0f, pTime);
        extraJumptime = 0.0f;

        anim = GetComponent<Animator>();
    }

    //=============================================================================================================================================//

    private bool IsSmall {
        get {
            // if both small colliders are enabled and big colliders are disabled then return true
            return hitSmall.enabled && trigSmall.enabled && !trigBig && !hitBig.enabled;
        }
        set {
            //if true then enable the small colliders
            trigSmall.enabled = value;
            hitSmall.enabled = value;
            //if true then disable the big colliders
            trigBig.enabled = !value;
            hitBig.enabled = !value;
            //if false then the reverse happens
        }
    }

    // isFacingRight can be gotten from outside the script
    public bool IsFacingRight() { return isFacingRight; }

    public Vector3 center {
        get {
            return trigSmall.bounds.center;
        }
    }

    //=============================================================================================================================================//

    public void CollectItem(string itemName) {

    }

    public void TakeDamage() {

    }

    //=============================================================================================================================================//

    private void FixedUpdate() {
        //do this once in the begining so i dont have to check multiple times
        axis.getDir();
        aBut.getInput();
        bBut.getInput();

        //flip the player accordingly
        checkForFlip();

        HandleAttack();

        //first is horizontal movement
        calculatePSpeed();
        HandleXMovement();

        //second is vertical movement
        calculateJumpHeight();
        HandleYMovement();

        //apply the new velocity
        rb.velocity = new Vector3(XVel.Amount, YVel, 0.0f);

        //updates the properties in the animation controller
        UpdateAnimVariables();
    }

    //=============================================================================================================================================//

    private void HandleAttack() {
        if (bBut.ButtonDown) {
            if (curPowerUp == powerups.fire) {
                Vector3 spawnPos = hitBig.bounds.center;
                spawnPos.y += hitBig.bounds.extents.y / 2.0f;
                GameObject ob = Instantiate(fireBall, spawnPos, Quaternion.Euler(Vector3.zero));
                ob.GetComponent<FireBallController>().IsFacingRight = isFacingRight;
            } else if (curPowerUp == powerups.leaf) {

            }
        }
    }

    //====================================================-------Movement-------===================================================================//

    private void checkForFlip() {
        //only flips the player if their holding a direction and also is moving in that direction
        //eg. one direction is held but mario is sliding the other way so mario wont flip
        if (isFacingRight && axis.Left && (XVel.Amount <= 0.0f || !cc.IsGrounded)) Flip();
        if (!isFacingRight && axis.Right && (XVel.Amount >= 0.0f || !cc.IsGrounded)) Flip();
    }

    private void calculatePSpeed() {
        //control Pspeed based off of how long b is held and if the player is moving faster than run speed
        if (bBut.ButtonHeld && XVel.Amount != 0.0f) {
            //first the max speed gets set to the players run speed
            if (XVel.Range < maxRunSpeed) {
                XVel.Range = maxRunSpeed;
            }
            //then once mario is running at full speed, the p meter starts going up
            if (XVel.Abs >= maxRunSpeed) {
                PMeter.Amount += Time.fixedDeltaTime;
            }
            //once the pMeter has filled, the max speed gets increased to a new speed
            if (PMeter.Amount == PMeter.Max) {
                XVel.Range = maxPSpeed;
            }
        } else {
            //if b is released or the speed is too slow, then the max is reset
            XVel.Range = maxWalkSpeed;
        }

        //if marios speed is lower than the max run speed start counting the pmeter down
        if (XVel.Abs < maxRunSpeed) {
            PMeter.Amount -= Time.fixedDeltaTime;
        }
    }

    private void HandleXMovement() {
        //if mario hits a wall then the speed gets set to 0
        if (cc.IsRightColliding && XVel.Amount > 0.0f) {
            XVel.Amount = 0.0f;
        } else if (cc.IsLeftColliding && XVel.Amount < 0.0f) {
            XVel.Amount = 0.0f;
        }

        //accelerate in the given direction as long as the speed is slower than the max
        if ((axis.Left ^ axis.Right) && XVel.Abs <= XVel.Range) {
            if (axis.Left) {
                XVel.Amount -= XAccel * Time.fixedDeltaTime;
            } else if (axis.Right) {
                XVel.Amount += XAccel * Time.fixedDeltaTime;
            }
        } else {
            //have the player decelerate back to 0
            if (XVel.Amount < -0.1f) XVel.Amount += XDeccel * Time.fixedDeltaTime;
            else if (XVel.Amount > 0.1f) XVel.Amount -= XDeccel * Time.fixedDeltaTime;
            //then check if the amount is within -0.1 to 0.1 and set to 0
            if (XVel.Abs <= 0.1) XVel.Amount = 0.0f;
        }
    }

    private void calculateJumpHeight() {
        //calculates jump height based off run speed
        if (cc.IsGrounded) {
            extraJumptime = extraJumpHeight * XVel.Abs / maxPSpeed * (riseTime / jumpHeight);
            if (YTime.Amount >= YTime.Max - 0.01f) {
                YTime.Amount = YTime.Max;
            }
            YTime.Max = riseTime + extraJumptime;
        }
    }

    private void HandleYMovement() {
        //reset the timer if player has pressed jump
        if (aBut.ButtonDown && cc.IsGrounded) {
            YTime.Amount = 0.0f;
            YVel = jumpHeight / riseTime;
        }

        //increase the timer and set the velocity to a constant speed until the timer is done
        if (YTime.Amount < YTime.Max) {
            YTime.Amount += Time.fixedDeltaTime;
            //YVel = jumpHeight / riseTime;
        }

        //this will end the timer imediately if the player lets go of th jump button
        if (!aBut.ButtonHeld && YTime.Amount >= (minJumpHeight / jumpHeight) * riseTime) {
            YTime.Amount = YTime.Max;
        }

        //this end the timer imediately if the players head hits something
        if (cc.IsTopColliding) {
            YTime.Amount = YTime.Max;
            YVel = -fallSpeed;
        }

        //this applies downwards velocity once the timer is done
        if (YTime.Amount >= YTime.Max) {
            YVel -= Time.fixedDeltaTime * fallAccel;
            if (rb.velocity.y < -fallSpeed) YVel = -fallSpeed / 2;
            if (cc.IsGrounded && YVel <= 0.0f) YVel = 0.0f;
        }
    }

    //=============================================================================================================================================//

    private void UpdateAnimVariables() {
        //tell if marios on the ground
        anim.SetBool("IsGrounded", cc.IsGrounded && YVel <= 0.0f);

        //the following are dependent on certain values

        //this is for the diffrent run speeds
        if (XVel.Abs >= maxPSpeed) {
            anim.SetBool("IsPRun", true);
        } else if (XVel.Abs >= maxRunSpeed) {
            anim.SetBool("IsPRun", false);

            anim.SetBool("IsRunning", true);
        } else if (XVel.Abs >= maxWalkSpeed) {
            anim.SetBool("IsPRun", false);
            anim.SetBool("IsRunning", false);

            anim.SetBool("IsFastWalking", true);
        } else if (XVel.Abs > 0.0f) {
            anim.SetBool("IsPRun", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsFastWalking", false);

            anim.SetBool("IsWalking", true);
        } else {
            anim.SetBool("IsPRun", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsFastWalking", false);
            anim.SetBool("IsWalking", false);
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

        //this is for marios pspeed jump
        if (PMeter.Amount >= PMeter.Max && !cc.IsGrounded) {
            anim.SetBool("PSpeedIsActive", true);
        } else if (cc.IsGrounded) {
            anim.SetBool("PSpeedIsActive", false);
        }

        //this is for determining when marios animation should be a jump or fall
        if (cc.IsGrounded && aBut.ButtonHeld) {
            anim.SetBool("HadJumped", true);
        } else if (cc.IsGrounded) {
            anim.SetBool("HadJumped", false);
        }

        anim.SetInteger("PowerUpState", (int)curPowerUp);
    }

}

//Debug.Log(collDir.Left() + ", " + collDir.Right() + ", " + collDir.Top() + ", " + collDir.Bottom());