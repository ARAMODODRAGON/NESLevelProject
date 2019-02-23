using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * I will be adding a list of all the classes that inherits from this one
 * 
 */ 
public abstract class Enemies : Agent {
    protected SpriteRenderer sp;

    protected int hitPoints;
    protected bool shouldColWithPlayer;

    protected override void Awake() {
        base.Awake();
        hitPoints = 1; ///default value
        shouldColWithPlayer = true;
    }

    protected override void Start() {
        base.Start();
        sp = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(string killType) {
        Destroy(gameObject);
    }

    //enemies will reset position when going off screen
    protected override void OnDeactivate() {
        shouldColWithPlayer = false;
        transform.position = SpawnPosition;
        col.enabled = false;
        sp.enabled = false;
        rb.simulated = false;
        ///so when the enemy resets and is back on screen, it will stay inactive
        if (CheckIsOnScreen) isActive = true;
    }

    protected override void OnActivate() {
        rb.simulated = true;
        col.enabled = true;
        sp.enabled = true;
        shouldColWithPlayer = true;
    }

    protected void OnOverlapEnter(Collider2D col) {
        if (hitPoints == 0) return;
        if (shouldColWithPlayer && col.tag.Equals("Player")) {
            Player.instance.TakeDamage(this);
        }
    }

}
