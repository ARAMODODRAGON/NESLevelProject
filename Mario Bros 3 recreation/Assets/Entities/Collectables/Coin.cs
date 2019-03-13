using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable {
    public bool flyUp;

    protected override void Start() {
        base.Start();
        itemName = "Coin";
        if (flyUp) {
            GetComponent<BoxCollider2D>().enabled = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = new Vector2(0f, 5f);
            Destroy(gameObject, 3f);
        } else {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == player.tag) {
            player.CollectItem(itemName);
        }
    }
}
