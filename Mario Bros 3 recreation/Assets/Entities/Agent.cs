using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * this is a class for non player entities
 * it is inherited by:
 * Collectable
 * (Not yet exists) Enemy
 */
public abstract class Agent : Entity {
    // player instance and transform
    protected Player player;
    protected Transform playerTransform;
    // wether or not this agent will automatically flip when hitting a wall
    protected bool canAutoflip;

    //its starting position
    protected Vector2 initialPosition;

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
        if (collDir == null) {
            Debug.LogError("No CollDir script found on child of " + name);
        } else {
            canAutoflip = true;
        }
        isFacingRight = false;
        initialPosition = transform.position;
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
        Vector3 distanceToPlayer = transform.position - playerTransform.position;
        //sets the x and y to their absolute values
        distanceToPlayer.x = Mathf.Abs(distanceToPlayer.x);
        distanceToPlayer.y = Mathf.Abs(distanceToPlayer.y);
        // flip
        if (canAutoflip) {
            CheckForFlip();
        }
        //checks the distances if they are too far
        if (distanceToPlayer.x < 6.0f || distanceToPlayer.y < 6.0f) {
            OnScreen();
        } else {
            OffScreen();
        }
    }
    protected abstract void OnScreen();
    protected abstract void OffScreen();

    protected virtual void CheckForFlip() {
        if (isFacingRight && collDir.IsRightColliding) {
            Flip();
        } else if (!isFacingRight && collDir.IsLeftColliding) {
            Flip();
        }
    }
}
