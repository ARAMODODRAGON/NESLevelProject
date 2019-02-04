using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollCheck : MonoBehaviour {
    
    [Header("Top Collider")]
    [Min(0.1f)]
    public float TopThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float TopCoverage = 0.9f;
    [Min(0.0f)]
    public float TopDisplacement;
    public LayerMask topLM;

    [Header("Left Collider")]
    [Min(0.1f)]
    public float LeftThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float LeftCoverage = 0.9f;
    [Min(0.0f)]
    public float LeftDisplacement;
    public LayerMask leftLM;

    [Header("Right Collider")]
    [Min(0.1f)]
    public float RightThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float RightCoverage = 0.9f;
    [Min(0.0f)]
    public float RightDisplacement;
    public LayerMask rightLM;

    [Header("Bottom Collider")]
    [Min(0.1f)]
    public float BottomThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float BottomCoverage = 0.9f;
    [Min(0.0f)]
    public float BottomDisplacement;
    public LayerMask bottomLM;

    [Space(10.0f)]
    [SerializeField] private bool drawBoxes = true;


    private BoxCollider2D thisCol;

    public bool IsColliding { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsTopColliding { get; private set; }
    public bool IsLeftColliding { get; private set; }
    public bool IsRightColliding { get; private set; }

    public bool CheckLeft { get; set; }
    public bool CheckRight { get; set; }
    public bool CheckTop { get; set; }
    public bool CheckBottom { get; set; }

    private void Start() {
        //set all properties to false
        IsColliding = false;
        IsGrounded = false;
        IsTopColliding = false;
        IsLeftColliding = false;
        IsRightColliding = false;

        //weather or not to check each side
        CheckLeft = true;
        CheckRight = true;
        CheckBottom = true;
        CheckTop = true;

        //the collider and coroutine
        StartCoroutine("CheckIfColl");
    }
    
    IEnumerator CheckIfColl() {
        while (true) {
            //will wait until right before fixedupdate happens to get new collisions
            yield return new WaitForFixedUpdate();
            
            //waits until theres an enabled box collider on the object
            if (thisCol == null) {
                foreach (BoxCollider2D col in GetComponents<BoxCollider2D>()) {
                    if (col.enabled) {
                        thisCol = col;
                    }
                }
                if(thisCol == null) {
                    continue;
                }
            }

            Vector2 colliderCenter = thisCol.bounds.center;
            Vector2 halfExtents = thisCol.bounds.extents;
            halfExtents.x += thisCol.edgeRadius;
            halfExtents.y += thisCol.edgeRadius;

            //side is only checked for if check is set to true
            if (CheckBottom) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x;
                boxCenter.y = colliderCenter.y - halfExtents.y - BottomThickness / 2.0f - BottomDisplacement;

                Vector2 boxSize;
                boxSize.x = halfExtents.x * 2.0f * BottomCoverage;
                boxSize.y = BottomThickness;

                //then caluculates and returns true if there was a collision
                IsGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0.0f, bottomLM);

                //now draw the box
                DrawBox(boxCenter, boxSize, Color.blue);
            }

            //side is only checked for if check is set to true
            if (CheckTop) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x;
                boxCenter.y = colliderCenter.y + halfExtents.y + TopThickness / 2.0f + TopDisplacement;

                Vector2 boxSize;
                boxSize.x = halfExtents.x * 2.0f * TopCoverage;
                boxSize.y = TopThickness;

                //then caluculates and returns true if there was a collision
                IsTopColliding = Physics2D.OverlapBox(boxCenter, boxSize, 0.0f, topLM);

                //now draw the box
                DrawBox(boxCenter, boxSize, Color.blue);
            }

            //side is only checked for if check is set to true
            if (CheckLeft) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x - halfExtents.x - LeftThickness / 2.0f - LeftDisplacement;
                boxCenter.y = colliderCenter.y;

                Vector2 boxSize;
                boxSize.x = LeftThickness;
                boxSize.y = halfExtents.x * 2.0f * LeftCoverage;

                //then caluculates and returns true if there was a collision
                IsLeftColliding = Physics2D.OverlapBox(boxCenter, boxSize, 0.0f, leftLM);

                //now draw the box
                DrawBox(boxCenter, boxSize, Color.blue);
            }

            //side is only checked for if check is set to true
            if (CheckRight) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x + halfExtents.x + RightThickness / 2.0f + RightDisplacement;
                boxCenter.y = colliderCenter.y;

                Vector2 boxSize;
                boxSize.x = RightThickness;
                boxSize.y = halfExtents.x * 2.0f * RightCoverage;

                //then caluculates and returns true if there was a collision
                IsRightColliding = Physics2D.OverlapBox(boxCenter, boxSize, 0.0f, rightLM);

                //now draw the box
                DrawBox(boxCenter, boxSize, Color.blue);
            }
        }
    }
    
    private void DrawBox(Vector2 center, Vector2 size, Color color) {
        if (drawBoxes) {
            //draw a box
            Debug.DrawLine(new Vector2(center.x - size.x / 2.0f, center.y + size.y / 2.0f),
                new Vector2(center.x - size.x / 2.0f, center.y - size.y / 2.0f), color);

            Debug.DrawLine(new Vector2(center.x + size.x / 2.0f, center.y + size.y / 2.0f),
                new Vector2(center.x + size.x / 2.0f, center.y - size.y / 2.0f), color);

            Debug.DrawLine(new Vector2(center.x - size.x / 2.0f, center.y + size.y / 2.0f),
                new Vector2(center.x + size.x / 2.0f, center.y + size.y / 2.0f), color);

            Debug.DrawLine(new Vector2(center.x - size.x / 2.0f, center.y - size.y / 2.0f),
                new Vector2(center.x + size.x / 2.0f, center.y - size.y / 2.0f), color);
        }
    }
}