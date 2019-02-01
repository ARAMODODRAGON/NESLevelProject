using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollDir : MonoBehaviour {
    private new Collider2D collider;

    private Vector2 halfSize;
    [Range(0.015f, 0.08f)]
    public float distFromCollider = 0.015f;
    [Range(0.1f, 0.2f)]
    public float distFromEdge = 0.1f;

    private bool collHasRun;

    //the variables that state weather the player is colliding or not
    private bool isColliding;
    private bool isLeftColliding;
    private bool isRightColliding;
    private bool isTopColliding;
    private bool isBottomColliding;

    private void Start() {
        collider = GetComponent<Collider2D>();
        if (collider == null) {
            Debug.LogError("No collider found");
        }

        halfSize = collider.bounds.extents;

        isColliding = false;
        isLeftColliding = false;
        isRightColliding = false;
        isTopColliding = false;
        isBottomColliding = false;
        collHasRun = false;
        StartCoroutine("CheckIfColl");
    }

    public bool IsColliding() { return isColliding; }
    public bool Top() { return isTopColliding; }
    public bool Bottom() { return isBottomColliding; }
    public bool Left() { return isLeftColliding; }
    public bool Right() { return isRightColliding; }

    IEnumerator CheckIfColl() {
        while (true) {
            yield return new WaitForFixedUpdate();
            
            if (!collHasRun) {
                isColliding = isLeftColliding = isRightColliding = isTopColliding = isBottomColliding = false;
            }

            collHasRun = false;
        }
    }
    
    private void OnCollisionStay2D(Collision2D col) {
        collHasRun = true;
        isColliding = isLeftColliding = isRightColliding = isTopColliding = isBottomColliding = false;
        Vector2 centerOfCollider = collider.bounds.center;
        bool coll = false;
        Vector2 start = Vector2.zero;
        Vector2 end = Vector2.zero;

        bool left_ = false, right_ = false, top_ = false, bottom_ = false;
        for (int i = 0; i < col.contactCount; i++) {
            Vector2 point = col.GetContact(i).point - centerOfCollider;
            if (point.x <= -halfSize.x + distFromEdge) {
                left_ = true;
            }
            if (point.x >= halfSize.x - distFromEdge) {
                right_ = true;
            }
            if (point.y <= -halfSize.y + distFromEdge) {
                bottom_ = true;
            }
            if (point.y >= halfSize.y - distFromEdge) {
                top_ = true;
            }
        }

        // right
        if (right_) {
            start = new Vector2(halfSize.x + distFromCollider, halfSize.y - distFromEdge) + centerOfCollider;
            end = new Vector2(halfSize.x + distFromCollider, -halfSize.y + distFromEdge) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end);
            if (coll) {
                isRightColliding = true;
            }
        }
        // left
        if (left_) {
            start = new Vector2(-halfSize.x - distFromCollider, halfSize.y - distFromEdge) + centerOfCollider;
            end = new Vector2(-halfSize.x - distFromCollider, -halfSize.y + distFromEdge) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end);
            if (coll) {
                isLeftColliding = true;
            }
        }
        // top
        if (top_) {
            start = new Vector2(-halfSize.x + distFromEdge, halfSize.y + distFromCollider) + centerOfCollider;
            end = new Vector2(halfSize.x - distFromEdge, halfSize.y + distFromCollider) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end);
            if (coll) {
                isTopColliding = true;
            }
        }
        // bottom
        if (bottom_) {
            start = new Vector2(-halfSize.x + distFromEdge, -halfSize.y - distFromCollider) + centerOfCollider;
            end = new Vector2(halfSize.x - distFromEdge, -halfSize.y - distFromCollider) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end);
            if (coll) {
                isBottomColliding = true;
            }
        }

        isColliding = isLeftColliding || isRightColliding || isTopColliding || isBottomColliding;
    }
}