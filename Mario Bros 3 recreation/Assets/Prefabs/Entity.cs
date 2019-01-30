using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    //player variables
    protected GameObject Player;
    protected Transform PlayerTransform;

    protected virtual void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.transform;
    }

    protected virtual void FixedUpdate() {
        //gets the distance to the player
        Vector2 distanceToPlayer = transform.position - PlayerTransform.position;
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
