﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : Entity {
    public bool IsFacingRight {
        get {
            return isFacingRight;
        }
        set {
            if (isFacingRight != value) {
                Flip();
            }
            isFacingRight = value;
        }
    }

    public float maxYSpeed;
    public float XSpeed;
    public float YAccel;

    private Meter YVel;
    private float XVel;
    
    private void Awake() {
        isFacingRight = false;
        IsFacingRight = true;
    }

    protected override void Start() {
        base.Start();
        YVel = new Meter(maxYSpeed);
        YVel.Amount = YVel.Min;

        StartCoroutine("FireballAnim");
    }

    IEnumerator FireballAnim() {
        while (true) {
            yield return new WaitForSeconds(1.0f/10.0f);

            if (IsFacingRight) {
                transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
            } else {
                transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            }  
        }
    }

    private void FixedUpdate() {

        YVel.Amount -= YAccel * Time.fixedDeltaTime;

        if (cc.IsGrounded && YVel.Amount <= 0.0f) {
            YVel.Amount = YVel.Max;
        }

        if (IsFacingRight) {
            XVel = XSpeed;
        } else {
            XVel = -XSpeed;
        }
        
        rb.velocity = new Vector2(XVel, YVel.Amount);
    }
}
