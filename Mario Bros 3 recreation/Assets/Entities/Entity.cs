using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * is inherited by:
 * Agent,
 * Player
 */
public abstract class Entity : MonoBehaviour {
    // components which all entities need
    protected Rigidbody2D rb;
    protected CollDir collDir;

    protected bool isFacingRight;

    protected virtual void Start() {
        //get components
        collDir = GetComponentInChildren<CollDir>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Flip() {
        //flip is used by almost all entities
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
    
}
