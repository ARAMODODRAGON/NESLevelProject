using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollDir : MonoBehaviour {
    private new Collider2D collider;

    private Vector2 halfSize;

    //the variables that state weather the player is colliding or not
    private bool isColliding;
    private bool isLeftColliding;
    private bool isRightColliding;
    private bool isTopColliding;
    private bool isBottomColliding;

    private void Start() {
        collider = GetComponent<Collider2D>();

        halfSize = collider.bounds.extents.normalized;

        isColliding = false;
        isLeftColliding = false;
        isRightColliding = false;
        isTopColliding = false;
        isBottomColliding = false;
    }

    public bool IsColliding() { return isColliding; }
    public bool Top() { return isTopColliding; }
    public bool Bottom() { return isBottomColliding; }
    public bool Left() { return isLeftColliding; }
    public bool Right() { return isRightColliding; }

    private void OnCollisionStay2D(Collision2D col) {
        Vector2 contactPoint;
        Vector2 centerOfCollider = collider.bounds.center;

        for (int i = 0; i < col.contactCount; i++) {
            contactPoint = col.GetContact(i).point - centerOfCollider;
            contactPoint.Normalize();

            if (contactPoint.x <= halfSize.x && contactPoint.x >= -halfSize.x) {
                if (contactPoint.y > 0.0f) {
                    isTopColliding = true;
                    continue;
                } else if (contactPoint.y < 0.0f) {
                    isBottomColliding = true;
                    continue;
                } else {
                    Debug.LogError("There wasnt a collision?");
                    continue;
                }
            } else if (contactPoint.x < 0.0f) {
                isLeftColliding = true;
                continue;
            } else if (contactPoint.x > 0.0f) {
                isRightColliding = true;
                continue;
            } else {
                Debug.LogError("There wasnt a collision?");
                continue;
            }
        }

        isColliding = isLeftColliding || isRightColliding || isTopColliding || isBottomColliding;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isLeftColliding = isRightColliding = isTopColliding = isBottomColliding = isColliding = false;
    }
}