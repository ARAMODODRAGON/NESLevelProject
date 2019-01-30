using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    //player variables
    protected GameObject player;
    protected Transform playerTransform;
    protected Rigidbody2D rb;

    protected Vector2 initialPosition;
    
    protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        rb = GetComponent<Rigidbody2D>();

        initialPosition = transform.position;
    }

    protected virtual void FixedUpdate() {
        //gets the distance to the player
        Vector2 distanceToPlayer = transform.position - playerTransform.position;
        //sets the x and y to their absolute values
        distanceToPlayer.x = Mathf.Abs(distanceToPlayer.x);
        distanceToPlayer.y = Mathf.Abs(distanceToPlayer.y);
        
        //checks the distances if they are too far
        if (distanceToPlayer.x < 6.0f || distanceToPlayer.y < 6.0f) {
            OnScreen();
        } else {
            OffScreen();
        }
    }
    protected abstract void OnScreen();
    protected abstract void OffScreen();
    
}
