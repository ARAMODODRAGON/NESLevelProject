using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
 * is inherited by:
 * Agent,
 * Player
 */
public abstract class Entity : MonoBehaviour {
    // components which all entities need
    protected Rigidbody2D rb;
    protected EntityCollider ec;
    protected MultiTrigger mt;

    protected bool isFacingRight;

    protected bool lastCol;

    protected virtual void Start() {
        //get components
        ec = GetComponent<EntityCollider>();
        rb = GetComponent<Rigidbody2D>();
        mt = GetComponent<MultiTrigger>();
        lastCol = true;
    }

    protected virtual void FixedUpdate() {
        /*bool thisCol = false;
        foreach (Collider2D col in ec.results) {
            if (col != null) thisCol |= true;
            if (col != null) OnOverlap(col);
        }
        if (!lastCol)
        foreach (Collider2D col in ec.results) {
            if (col != null) OnOverlapEnter(col);
        }//*/
    }

    protected void Flip() {
        //flip is used by almost all entities
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    protected virtual void OnOverlap(Collider2D col) { }
    protected virtual void OnOverlapEnter(Collider2D col) { }
}
