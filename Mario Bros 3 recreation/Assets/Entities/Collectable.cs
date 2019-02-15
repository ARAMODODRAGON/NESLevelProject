using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * is inherited by:
 * Mushroom
 * FireFlower
 * FrogSuit
 * Leaf
 * (Not yet exists) Coin
 */
public abstract class Collectable : Agent {
    protected bool isInBlock;
    private float riseLength;

    protected string itemName;
    [HideInInspector]
    public bool goUp;

    protected override void Awake() {
        base.Awake();
        goUp = false;
    }

    protected override void Start() {
        isInBlock = true;
        itemName = "item";
        base.Start();
        riseLength = 0.9f;
        rb.simulated = false;
    }

    protected override void ActiveUpdate() {
        if (isInBlock && goUp) {
            RunExitBlockAnimation();
        }
    }

    private void RunExitBlockAnimation() {
        Vector2 newPos = transform.position;
        newPos.y += Time.fixedDeltaTime / riseLength;
        transform.position = newPos;

        if (transform.position.y >= (SpawnPosition.y + 1.0f)) {
            isInBlock = false;
            rb.simulated = true;
        }
    }

    protected override void InactiveUpdate() {
        //delete the PowerUp
        //this is overriden by the coin as it is not a powerup but is a collectable
        Destroy(gameObject);
    }

    protected override void OnOverlap(Collider2D coll) {
        if (coll.gameObject.tag.Equals(player.tag)) {
            player.CollectItem(itemName);
            Destroy(gameObject);
        }
    }
}
