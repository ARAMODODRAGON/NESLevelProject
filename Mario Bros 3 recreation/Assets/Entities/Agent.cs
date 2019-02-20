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
    // components
    protected Player player;
    protected CameraController cameraInst;
    protected BoxCollider2D col;
    // wether or not this agent will automatically flip when hitting a wall
    protected bool CanCheckForFlip;
    protected bool isActive;

    protected bool destroyOnExit;

    // its starting position
    protected Vector2 SpawnPosition;

    //===============================================================================================================================================//

    protected virtual void Awake() {
        SpawnPosition = transform.position;
        col = GetComponent<BoxCollider2D>();
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
        cameraInst = CameraController.instance;

        //does and initial check to see if is on screen to decide if it should start active

        //gets the distance to the player
        Vector3 distanceToCamera = transform.position - cameraInst.transform.position;
        //sets the x and y to their absolute values
        distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
        distanceToCamera.y = Mathf.Abs(distanceToCamera.y);
        if (distanceToCamera.x > cameraInst.ScreenSize.x || distanceToCamera.y > cameraInst.ScreenSize.y) {
            isActive = false;
        } else {
            isActive = true;
        }
    }

    //===============================================================================================================================================//

    ///the way this is gonna work is that if one agent is not going to use a function then it can just override fixedupdate and run the functions it needs
    protected override void FixedUpdate() {
        base.FixedUpdate();

        //flip
        CheckForFlip();

        //checks if this entity is visible on screen and runs the event functions
        CallEvents();
    }

    //===============================================================================================================================================//

    protected bool CheckIsOnScreen {
        get {
            //gets the distance to the camera
            Vector3 distanceToCamera = transform.position - cameraInst.transform.position;
            //sets the x and y to their absolute values
            distanceToCamera.x = Mathf.Abs(distanceToCamera.x);
            distanceToCamera.y = Mathf.Abs(distanceToCamera.y);

            return !(distanceToCamera.x > cameraInst.ScreenSize.x || distanceToCamera.y > cameraInst.ScreenSize.y);
        }
    }

    protected virtual void CheckForFlip() {
        //flips the agent automatically using collision
        if (isFacingRight && ec.IsRight) {
            Flip();
        } else if (!isFacingRight && ec.IsLeft) {
            Flip();
        }
    }

    //checks if is onscreen or not and calls each function accordingly
    ///ActiveUpdate and InactiveUpdate are called every fixed update when the entity is on/offscreen
    ///OnDeactivate is when it leaves the screen
    ///OnActivate is when it appears on screen
    protected virtual void CallEvents() {
        if (!CheckIsOnScreen) {

            ///call this only once it leaves the screen
            if (isActive) {
                isActive = false; ///prevents from running more than once
                OnDeactivate();
            }

            ///called every fixed update if is off screen
            if (!isActive) InactiveUpdate();

            ///used to destroy an object if it leaves the screen
            if (destroyOnExit) Destroy(gameObject);
        } else {

            ///call when entering the screen
            if (!isActive) {
                isActive = true; ///prevents from running more than once
                OnActivate();
            }

            ///called every fixed update if is on screen
            if (isActive) ActiveUpdate();
        }
    }

    protected virtual void OnDeactivate() { }
    protected virtual void OnActivate() { }
    protected virtual void InactiveUpdate() { }
    protected virtual void ActiveUpdate() { }

}
