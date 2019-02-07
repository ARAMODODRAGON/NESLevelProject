using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * this is a class for non player entities
 * it is inherited by:
 * Collectable
 * Enemies
 */
public abstract class Agent : Entity {
    // player instance and transform
    protected Player player;
    protected Transform playerTransform;
    protected Transform cameraT;
    private bool isOnScreen;
    // wether or not this agent will automatically flip when hitting a wall
    protected bool canAutoflip;
    protected bool isActive;

    //its starting position
    protected Vector2 initialPosition;

    protected virtual void Awake() {
        isFacingRight = false;
        isActive = true;
    }

    protected override void Start() {
        //calls start on entity first
        base.Start();
        //gets the player instance
        //if it cant find it then it starts a coroutine which is almost garunteed to find it
        //then it calls found player to say that the player exists by this point
        player = Player.instance;
        if (player == null) {
            StartCoroutine("GetPlayerInstance");
        } else {
            FoundPlayer();
        }
        //gets the player transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (cc == null) {
            Debug.LogError("No CollDir script found on child of " + name);
        } else {
            canAutoflip = true;
        }
        initialPosition = transform.position;
        
        //gets the distance to the player
        Vector3 distanceToCamera = transform.position - cameraT.position;
        //sets the x and y to their absolute values
        distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
        distanceToCamera.y = Mathf.Abs(distanceToCamera.y);
        cameraT = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (distanceToCamera.x > 8.0f || distanceToCamera.y > 8.0f) {
            isOnScreen = false;
        } else {
            isOnScreen = true;
        }
    }

    IEnumerator GetPlayerInstance() {
        yield return new WaitForEndOfFrame();
        player = Player.instance;
        if (player == null) {
            Debug.LogError("Could not find player");
        } else {
            FoundPlayer();
        }
    }

    protected virtual void FoundPlayer() { }

    protected void FixedUpdate() {
        //gets the distance to the player
        Vector3 distanceToCamera = transform.position - cameraT.position;
        //sets the x and y to their absolute values
        distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
        distanceToCamera.y = Mathf.Abs(distanceToCamera.y);
        // flip
        if (canAutoflip) {
            CheckForFlip();
        }
        //checks the distances if they are too far
        if (distanceToCamera.x > 8.0f || distanceToCamera.y > 8.0f) {
            if (isOnScreen) ExitedScreen();
            isOnScreen = false;
            OffScreen();
        } else {
            if (!isOnScreen) EnteredScreen();
            isOnScreen = true;
            if (isActive) OnScreen();
        }
    }
    protected abstract void OnScreen();
    protected abstract void OffScreen();
    protected virtual void EnteredScreen() {}
    protected virtual void ExitedScreen() {}

    protected virtual void CheckForFlip() {
        if (isFacingRight && cc.IsRightColliding) {
            Flip();
        } else if (!isFacingRight && cc.IsLeftColliding) {
            Flip();
        }
    }
}
