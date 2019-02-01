using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : Entity {

    protected Player player;
    protected Transform playerTransform;
    protected bool canAutoflip;

    protected Vector2 initialPosition;

    protected override void Start() {
        base.Start();
        player = Player.instance;
        if (player == null) {
            StartCoroutine("GetPlayerInstance");
        }
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
        }
    }

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
        if (isFacingRight && collDir.Right()) {
            Flip();
        } else if (!isFacingRight && collDir.Left()) {
            Flip();
        }
    }
}
