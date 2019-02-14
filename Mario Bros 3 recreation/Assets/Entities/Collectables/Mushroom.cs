using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {
    private float speed;
    
    protected override void Start() {
        base.Start();
        itemName = "Mushroom";
        speed = 3.0f;
        isFacingRight = !player.IsFacingRight();
    }
    
    protected override void ActiveUpdate() {
        base.ActiveUpdate();
        if (!isInBlock) {
            MoveHorizontal();
        }
    }

    private void MoveHorizontal() {
        Vector2 vel = Vector2.zero;
        if (isFacingRight) {
            vel.x = speed;
        } else {
            vel.x = -speed;
        }
        vel.y = rb.velocity.y;
        rb.velocity = vel;
    }
    
}
