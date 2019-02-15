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
    protected EntityCollider ec;

    protected bool isFacingRight;

    protected virtual void Start() {
        //get components
        ec = GetComponent<EntityCollider>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate() {
        foreach (Collider2D col in ec.results) {
            if (col != null) OnOverlap(col);
        }
    }

    protected void Flip() {
        //flip is used by almost all entities
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    protected virtual void OnOverlap(Collider2D col) { }
}
