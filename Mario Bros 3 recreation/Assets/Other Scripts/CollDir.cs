using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollDir : MonoBehaviour {

    [SerializeField] LayerMask lm;
    
    [Range(0.02f, 2.0f)]
    public float distFromCollider = 0.02f;
    [Range(-0.2f, 0.2f)]
    public float distFromEdge = 0.01f;
    
    private Collider2D thisCol;
    
    //the variables that state weather the player is colliding or not
    private bool isColliding;
    private bool isLeftColliding;
    private bool isRightColliding;
    private bool isTopColliding;
    private bool isBottomColliding;

    private void Start() {
        isColliding = false;
        isLeftColliding = false;
        isRightColliding = false;
        isTopColliding = false;
        isBottomColliding = false;
        thisCol = GetComponent<Collider2D>();
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
            
            Vector2 halfSize = thisCol.bounds.extents;
            isColliding = isLeftColliding = isRightColliding = isTopColliding = isBottomColliding = false;
            Vector2 centerOfCollider = thisCol.bounds.center;
            bool coll = false;
            Vector2 start = Vector2.zero;
            Vector2 end = Vector2.zero;

            // right
            start = new Vector2(halfSize.x + distFromCollider, halfSize.y - distFromEdge) + centerOfCollider;
            end = new Vector2(halfSize.x + distFromCollider, -halfSize.y + distFromEdge) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end, lm);
            if (coll) {
                isRightColliding = true;
            }

            // left
            start = new Vector2(-halfSize.x - distFromCollider, halfSize.y - distFromEdge) + centerOfCollider;
            end = new Vector2(-halfSize.x - distFromCollider, -halfSize.y + distFromEdge) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end, lm);
            if (coll) {
                isLeftColliding = true;
            }

            // top
            start = new Vector2(-halfSize.x + distFromEdge, halfSize.y + distFromCollider) + centerOfCollider;
            end = new Vector2(halfSize.x - distFromEdge, halfSize.y + distFromCollider) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end, lm);
            if (coll) {
                isTopColliding = true;
            }

            // bottom
            start = new Vector2(-halfSize.x + distFromEdge, -halfSize.y - distFromCollider) + centerOfCollider;
            end = new Vector2(halfSize.x - distFromEdge, -halfSize.y - distFromCollider) + centerOfCollider;
            Debug.DrawLine(start, end);
            coll = Physics2D.Linecast(start, end, lm);
            if (coll) {
                isBottomColliding = true;
            }


            isColliding = isLeftColliding || isRightColliding || isTopColliding || isBottomColliding;
        }
    }
    
    private void OnCollisionStay2D(Collision2D col) {
        thisCol = col.otherCollider;
    }
}