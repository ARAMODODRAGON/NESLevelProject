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

    protected string itemName;

    protected override void Start() {
        itemName = "item";
        base.Start();
    }

    protected abstract override void OnScreen();

    protected override void OffScreen() {
        //delete the PowerUp
        //this is overriden by the coin as it is not a powerup
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag.Equals(player.tag)) {
            player.CollectItem(itemName);
            Destroy(gameObject);
        }
    }
}
