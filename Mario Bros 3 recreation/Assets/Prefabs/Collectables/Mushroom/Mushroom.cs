using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {
    private bool isInBlock;
    private bool isFacingRight;

    private float speed;
    private float riseLength;

    protected override void Start() {
        base.Start();
        isFacingRight = Player.hasLastMovedRight;
        isInBlock = true;
        rb.simulated = false;
        speed = 3.0f;
        riseLength = 0.9f;
    }

    protected override void OnScreen() {
        if(isInBlock) {
            RunExitBlockAnimation();
        } else {
            MoveHorizontal();
        }
    }

    private void RunExitBlockAnimation() {
        Vector2 newPos = transform.position;
        newPos.y += Time.fixedDeltaTime/ riseLength;
        transform.position = newPos;

        if(transform.position.y >= (initialPosition.y + 1.0f)) {
            isInBlock = false;
            rb.simulated = true;
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
