using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    protected Rigidbody2D rb;
    protected CollDir collDir;

    protected bool isFacingRight;

    protected virtual void Start() {
        collDir = GetComponentInChildren<CollDir>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1.0f;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
    
}
