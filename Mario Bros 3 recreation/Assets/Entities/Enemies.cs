﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : Agent {
    private BoxCollider2D ChildCol;
    protected SpriteRenderer sp;
    private bool canBecomeActive;


    protected override void Start() {
        base.Start();
        ChildCol = GetComponentInChildren<BoxCollider2D>();
        canBecomeActive = true;
    }

    protected abstract override void OnScreen();

    //enemies will reset position when going off screen
    protected override void ExitedScreen() {
        transform.position = initialPosition;
        isActive = false;
        ChildCol.enabled = false;
        sp.enabled = false;
    }

    protected override void OffScreen() {
        //gets the distance to the player
        Vector3 distanceToCamera = transform.position - cameraT.position;
        //sets the x and y to their absolute values
        distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
        distanceToCamera.y = Mathf.Abs(distanceToCamera.y);
        if (distanceToCamera.x > 8.0f || distanceToCamera.y > 8.0f) {
            canBecomeActive = true;
        } else {
            canBecomeActive = false;
        }
    }

    protected override void EnteredScreen() {
        if (canBecomeActive) {
            isActive = true;
            ChildCol.enabled = true;
            sp.enabled = true;
        }
    }
}
