using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * this is a class for non player entities
 * it is inherited by:
 * Collectable
 * Enemies
 */
public class Agent : Entity {
    // player instance and transform
    protected Player player;
    new protected CameraController camera; // the new keyword is to hide the inherited camera component as its not required here
    // wether or not this agent will automatically flip when hitting a wall
    protected bool CanCheckForFlip;
    protected bool isActive;
    private bool isOnScreen;

    protected bool destroyOnExit;

    // its starting position
    protected Vector2 SpawnPosition;

    //===============================================================================================================================================//

    protected virtual void Awake() {
        isFacingRight = false;
        isActive = true;
        destroyOnExit = false;
        CanCheckForFlip = true;
    }

    protected override void Start() {
        //calls start on entity first
        base.Start();

        //gets the player instance
        //this is a public static variable so only one instance of it can exsists at any one point
        //the instance is gotten in awake in the player script so agents are all able to get it from start
        player = Player.instance;
        //this also applies to the camera
        camera = CameraController.instance;

        //gets the distance to the player
        Vector3 distanceToCamera = transform.position - camera.transform.position;
        //sets the x and y to their absolute values
        distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
        distanceToCamera.y = Mathf.Abs(distanceToCamera.y);
        if (distanceToCamera.x > 8.5f || distanceToCamera.y > 8.5f) {
            isOnScreen = false;
        } else {
            isOnScreen = true;
        }
    }

    //===============================================================================================================================================//

    ///the way this is gonna work is that if one agent is not going to use a function then it can just override fixedupdate and run the functions it needs
    protected void FixedUpdate() {
        //flip
        CheckForFlip();
        
        //checks if this entity is visible on screen and runs the event functions
        CheckIsOnScreen();
    }

    //===============================================================================================================================================//

    protected virtual void CheckForFlip() {
        //flips the agent automatically using collision
        if (isFacingRight && cc.IsRightColliding) {
            Flip();
        } else if (!isFacingRight && cc.IsLeftColliding) {
            Flip();
        }
    }

    protected virtual void CheckIsOnScreen() {
        //gets the distance to the camera
        Vector3 distanceToCamera = transform.position - camera.transform.position;
        //sets the x and y to their absolute values
        distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
        distanceToCamera.y = Mathf.Abs(distanceToCamera.y);

        //checks if is onscreen or not and calls each function accordingly
        ///OnScreen and OffScreen are obvious
        ///ExitedScreen is when it leaves the screen
        ///EnteredScreen is when it appears on screen
        if (distanceToCamera.x > 10f || distanceToCamera.y > 10f) {
            ///call this only once it leaves the screen
            if (isOnScreen) OnDeactivate();

            ///this is if the agent is teleported onto the screen before it calls off screen
            ///this prevents an enemy, for example, from suddenly appearing on screen
            if (distanceToCamera.x <= 10f || distanceToCamera.y <= 10f) {
                isOnScreen = true;
            } else {
                isOnScreen = false;
            }

            ///called every fixed update if is off screen
            InactiveUpdate();

            ///used to destroy an object if it leaves the screen
            if (destroyOnExit) Destroy(gameObject);

        } else {
            if (!isOnScreen) OnActivate();
            isOnScreen = true;
            if (isActive) ActiveUpdate();
        }
    }

    protected virtual void OnDeactivate() {}
    protected virtual void OnActivate() {}
    protected virtual void InactiveUpdate() {}
    protected virtual void ActiveUpdate() {}

    
}
