using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected CollDir collDir;

    protected Vector2 initialPosition;
    protected bool isFacingRight;

    protected virtual void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        collDir = GetComponentInChildren<CollDir>();
        if(collDir == null) {
            Debug.LogError("No CollDir script found on child of " + name);
        }
        isFacingRight = false;
        initialPosition = transform.position;
    }

    protected void FixedUpdate() {
        //gets the distance to the player
        Vector3 distanceToPlayer = transform.position - playerTransform.position;
        //sets the x and y to their absolute values
        distanceToPlayer.x = Mathf.Abs(distanceToPlayer.x);
        distanceToPlayer.y = Mathf.Abs(distanceToPlayer.y);

        //checks the distances if they are too far
        if (distanceToPlayer.x < 6.0f || distanceToPlayer.y < 6.0f) {
            OnScreen();
        } else {
            OffScreen();
        }
        CheckForFlip();
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
    protected void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
    }
    
}
