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
    public enum Powerups { dead, small, big, fire, leaf, frog };
    public Powerups curPowerUp;
    public int currentPow {
        get {
            return (int)curPowerUp;
        }
    }
    public bool isTransitioning;
    private bool isCrouching;
    private bool isHover;
    private bool isAttack;
    private bool canAttack;

    //movement variables
    [Header("X Movement Variables")]
    public float maxWalkSpeed;
    public float maxRunSpeed;
    public float maxPSpeed;
    public float pTime;
    public float XAccel;
    public float XDeccel;
    [Header("Y Movement Variables")]
    public float jumpHeight;
    public float riseTime;
    public float fallSpeed;
    public float fallAccel;
    public float minJumpHeight;
    public float extraJumpHeight;
    private float extraJumptime;
    public float bounceHeight;

    private Meter XVel;
    private Meter YTime;
    private Meter PMeter;
    public float YVel { get; private set; }

    //animation variables
    private Animator anim;
    private bool hadJumped;

    public GameObject fireBall;

    //=============================================================================================================================================//

    private void Awake() {
        curPowerUp = Powerups.small;
        if (instance != null) {
            Debug.LogError("There is already one player in the scene");
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    protected override void Start() {
        base.Start();
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
            e.TakeDamage("Stomp");
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

    //=============================================================================================================================================//

    private void FixedUpdate() {
        if (!isTransitioning && curPowerUp != Powerups.dead) {

            //do this once in the begining so i dont have to check multiple times
            axis.getDir();
            aBut.getInput();
            bBut.getInput();

            //flip the player accordingly
            CheckForFlip();
            //Debug.Log(axis.Down + ", " + axis.Left + ", " + axis.Right);

            CheckForCrouch();
            AttackCheck();

            //first is horizontal movement
            calculatePSpeed();
            HandleXMovement();

            //second is vertical movement
            calculateJumpHeight();
            HandleYMovement();

            //apply the new velocity
            rb.velocity = new Vector3(XVel.Amount, YVel, 0.0f);
        } else if (isTransitioning) {
            rb.velocity = Vector3.zero;
        }

        //updates the properties in the animation controller
        UpdateAnimVariables();
    }

    //=============================================================================================================================================//

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

    //====================================================-------Movement-------===================================================================//

    private void CheckForFlip() {
        //only flips the player if their holding a direction and also is moving in that direction
        //eg. one direction is held but mario is sliding the other way so mario wont flip
        if (isFacingRight && axis.Left && (XVel.Amount <= 0.0f || !cc.IsGrounded)) Flip();
        if (!isFacingRight && axis.Right && (XVel.Amount >= 0.0f || !cc.IsGrounded)) Flip();

        //this is for when they land and they arnt facing the right direction
        if (XVel.Amount < 0.0f && isFacingRight && cc.IsGrounded) Flip();
        if (XVel.Amount > 0.0f && !isFacingRight && cc.IsGrounded) Flip();
    }

    private void CheckForCrouch() {
        if (curPowerUp != Powerups.small && cc.IsGrounded) {
            if (axis.Down && !axis.Left && !axis.Right) {
                isCrouching = true;
                IsSmall = true;
            } else {
                isCrouching = false;
                IsSmall = false;
            }
        }
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

}

//Debug.Log(collDir.Left() + ", " + collDir.Right() + ", " + collDir.Top() + ", " + collDir.Bottom());