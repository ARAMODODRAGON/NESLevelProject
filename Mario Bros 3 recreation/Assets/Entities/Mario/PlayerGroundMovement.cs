using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Entity {

    //movement variables
    [Header("Ground Movement")]
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float maxPSpeed;
    [SerializeField] private float pTime;
    [SerializeField] private float XAccel;
    [SerializeField] private float XDeccel;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float riseTime;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float fallAccel;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float extraJumpHeight;
    [SerializeField] private float bounceHeight;
    private float extraJumptime;

    private void GroundUpdate() {
        //first is horizontal movement
        CalculatePSpeed();
        HandleXGroundMovement();

        //second is vertical movement
        CalculateJumpHeight();
        HandleYGroundMovement();
    }

    private void CheckForCrouch() {
        if (curPowerUp != Powerups.small && ec.IsGrounded) {
            if (axis.Down && !axis.Left && !axis.Right) {
                isCrouching = true;
                IsSmall = true;
            } else {
                isCrouching = false;
                IsSmall = false;
            }
        }
    }

    private void CalculatePSpeed() {
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

    private void HandleXGroundMovement() {
        //if mario hits a wall then the speed gets set to 0
        if (ec.IsRight && XVel.Amount > 0.0f) {
            XVel.Amount = 0.0f;
        } else if (ec.IsLeft && XVel.Amount < 0.0f) {
            XVel.Amount = 0.0f;
        }

        //accelerate in the given direction as long as the speed is slower than the max
        if ((axis.Left ^ axis.Right) && XVel.Abs <= XVel.Range) {
            if (axis.Left) {
                XVel.Amount -= XAccel * Time.fixedDeltaTime;
            } else if (axis.Right) {
                XVel.Amount += XAccel * Time.fixedDeltaTime;
            }
        } else if (ec.IsGrounded) {
            //have the player decelerate back to 0
            if (XVel.Amount < -0.1f) XVel.Amount += XDeccel * Time.fixedDeltaTime;
            else if (XVel.Amount > 0.1f) XVel.Amount -= XDeccel * Time.fixedDeltaTime;
            //then check if the amount is within -0.1 to 0.1 and set to 0
            if (XVel.Abs <= 0.1) XVel.Amount = 0.0f;
        }
    }

    private void CalculateJumpHeight() {
        //calculates jump height based off run speed
        if (ec.IsGrounded) {
            extraJumptime = extraJumpHeight * XVel.Abs / maxPSpeed * (riseTime / jumpHeight);
            if (YTime.Amount >= YTime.Max - 0.01f) {
                YTime.Amount = YTime.Max;
            }
            YTime.Max = riseTime + extraJumptime;
        }
    }

    private void HandleYGroundMovement() {
        //reset the timer if player has pressed jump
        if (aBut.ButtonDown && ec.IsGrounded) {
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
        if (ec.IsCeiling) {
            YTime.Amount = YTime.Max;
            YVel = -fallSpeed;
        }

        //this applies downwards velocity once the timer is done
        if (YTime.Amount >= YTime.Max) {
            YVel -= Time.fixedDeltaTime * fallAccel;
            if (rb.velocity.y < -fallSpeed) YVel = -fallSpeed / 2;
            if (ec.IsGrounded && YVel <= 0.0f) YVel = 0.0f;
        }
    }

}
