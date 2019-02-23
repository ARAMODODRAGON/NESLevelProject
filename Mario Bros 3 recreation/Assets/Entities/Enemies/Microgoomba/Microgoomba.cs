using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microgoomba : Enemies {

    [HideInInspector] public bool isAttached;

    protected override void Awake() {
        base.Awake();
        isAttached = false;
        Paragoomba.allMG.Add(this);
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        rb.velocity = new Vector2(rb.velocity.x, 5f);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Paragoomba.allMG.Remove(this);
    }
}
