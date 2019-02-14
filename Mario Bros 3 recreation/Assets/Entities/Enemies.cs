using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : Agent {
    protected BoxCollider2D ChildCol;
    protected SpriteRenderer sp;

    protected override void Start() {
        base.Start();
        ChildCol = GetComponentInChildren<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(string killType) {
        Destroy(gameObject);
    }

    //enemies will reset position when going off screen
    protected override void OnDeactivate() {
        transform.position = SpawnPosition;
        isActive = false;
        ChildCol.enabled = false;
        sp.enabled = false;
    }

    protected override void OnActivate() {
        isActive = true;
        ChildCol.enabled = true;
        sp.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.tag.Equals("Player")) {
            Player.instance.TakeDamage(this);
        }
    }

}
