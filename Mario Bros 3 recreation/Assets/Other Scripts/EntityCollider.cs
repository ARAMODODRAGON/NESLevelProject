using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * created by Domara Shlimon
 * ver 3.0
 */
[RequireComponent(typeof(BoxCollider2D))]
public class EntityCollider : MonoBehaviour {

    public int resultsSize = 1;

    [Header("MainBox")]
    public bool CheckMain = true;
    public LayerMask mainMask;
    public Vector2 percentCoverage = Vector2.one;

    [Header("Ceiling Check")]
    public bool CheckCeil = true;
    [Min(0.1f)]
    public float TopThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float TopCoverage = 0.9f;
    public float TopDisplacement = 0f;
    public LayerMask topLM;

    [Header("Ground Check")]
    public bool CheckGround = true;
    [Min(0.1f)]
    public float BottomThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float BottomCoverage = 0.9f;
    public float BottomDisplacement = 0f;
    public LayerMask bottomLM;

    [Header("Left Check")]
    public bool CheckLeft = true;
    [Min(0.1f)]
    public float LeftThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float LeftCoverage = 0.9f;
    public float LeftDisplacement = 0f;
    public LayerMask leftLM;

    [Header("Right Check")]
    public bool CheckRight = true;
    [Min(0.1f)]
    public float RightThickness = 0.09f;
    [Range(0.0f, 0.9f)]
    public float RightCoverage = 0.9f;
    public float RightDisplacement = 0f;
    public LayerMask rightLM;

    [Space(10.0f)]
    [SerializeField] private bool DrawGizmos = true;

    [HideInInspector] public BoxCollider2D box;
    [HideInInspector] public Collider2D[] results;

    private Collision2D ptp;
    private bool lastRes;

    public bool IsColliding { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsCeiling { get; private set; }
    public bool IsLeft { get; private set; }
    public bool IsRight { get; private set; }
    public bool IsSide {
        get {
            return IsLeft || IsRight;
        }
    }

    private void Awake() {
        box = GetComponent<BoxCollider2D>();
        results = new Collider2D[resultsSize];
        StartCoroutine(CollCheck());
        lastRes = false;
        IsColliding = false;
    }

    IEnumerator CollCheck() {
        while (true) {
            yield return new WaitForFixedUpdate();

            Vector2 colliderCenter = box.bounds.center;
            Vector2 halfExtents = box.bounds.extents;
            halfExtents.x += box.edgeRadius;
            halfExtents.y += box.edgeRadius;

            #region Ceiling Check

            //side is only checked for if check is set to true
            if (CheckCeil) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x;
                boxCenter.y = colliderCenter.y + halfExtents.y + TopThickness / 2f + TopDisplacement;

                Vector2 boxSize;
                boxSize.x = halfExtents.x * 2.0f * TopCoverage;
                boxSize.y = TopThickness;

                //then caluculates and returns true if there was a collision
                IsCeiling = Physics2D.OverlapBox(boxCenter, boxSize, 0f, topLM);
            }

            #endregion

            #region Ground Check

            //side is only checked for if check is set to true
            if (CheckGround) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x;
                boxCenter.y = colliderCenter.y - halfExtents.y - BottomThickness / 2f - BottomDisplacement;

                Vector2 boxSize;
                boxSize.x = halfExtents.x * 2.0f * BottomCoverage;
                boxSize.y = BottomThickness;

                //then caluculates and returns true if there was a collision
                IsGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0.0f, bottomLM);
            }

            #endregion

            #region LeftSide Check

            //side is only checked for if check is set to true
            if (CheckLeft) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x - halfExtents.x - LeftThickness / 2f - LeftDisplacement;
                boxCenter.y = colliderCenter.y;

                Vector2 boxSize;
                boxSize.x = LeftThickness;
                boxSize.y = halfExtents.y * 2.0f * LeftCoverage;

                //then caluculates and returns true if there was a collision
                IsLeft = Physics2D.OverlapBox(boxCenter, boxSize, 0.0f, leftLM);
            }

            #endregion

            #region RightSide Check

            //side is only checked for if check is set to true
            if (CheckRight) {
                //sets the center and size of this box
                Vector2 boxCenter;
                boxCenter.x = colliderCenter.x + halfExtents.x + RightThickness / 2f + RightDisplacement;
                boxCenter.y = colliderCenter.y;

                Vector2 boxSize;
                boxSize.x = RightThickness;
                boxSize.y = halfExtents.y * 2.0f * RightCoverage;

                //then caluculates and returns true if there was a collision
                IsRight = Physics2D.OverlapBox(boxCenter, boxSize, 0f, rightLM);
            }

            #endregion

            #region Main Collider Check

            //now to check the main area
            if (CheckMain) {
                results = new Collider2D[resultsSize];
                int i = Physics2D.OverlapAreaNonAlloc(colliderCenter - halfExtents * percentCoverage,
                    colliderCenter + halfExtents * percentCoverage, results, mainMask);

                if (i != 0 && !lastRes) {
                    foreach (Collider2D item in results) {
                        if (item == null) continue;
                        SendMessage("OnOverlapEnter", item, SendMessageOptions.DontRequireReceiver);
                    }
                }

                if (i != 0) {
                    foreach (Collider2D item in results) {
                        if (item == null) continue;
                        SendMessage("OnOverlapStay", item, SendMessageOptions.DontRequireReceiver);
                    }
                }

                if (i == 0 && lastRes) {
                    foreach (Collider2D item in results) {
                        if (item == null) continue;
                        SendMessage("OnOverlapExit", item, SendMessageOptions.DontRequireReceiver);
                    }
                }
                lastRes = i == 0;
            }

            #endregion
        }
    }

    private IEnumerator OnCollisionStay2D(Collision2D col) {
        yield return new WaitForFixedUpdate();
        IsColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        IsColliding = false;
    }

    //==============================================================================================================================//

    private void OnDrawGizmosSelected() {
        if (!DrawGizmos) return;

        if (box == null) box = GetComponent<BoxCollider2D>();

        Vector2 boxCenter, boxSize, center, size;
        boxCenter = transform.position + (Vector3)box.offset;
        boxSize.x = box.size.x + box.edgeRadius * 2f;
        boxSize.y = box.size.y + box.edgeRadius * 2f;

        //sets the colors
        Color outline = Color.white;
        outline.r = 0.1f;
        outline.g = 0.6f;
        outline.b = 0.9f;
        outline.a = 1f;
        Color fill = outline;
        fill.a = 0.3f;

        if (CheckMain) {
            //draw the main box
            Gizmos.color = fill;
            Gizmos.DrawCube(boxCenter, boxSize * percentCoverage);
            Gizmos.color = outline;
            Gizmos.DrawWireCube(boxCenter, boxSize * percentCoverage);
        }

        //sets colors
        outline = Color.red;
        fill = outline;
        fill.a = 0.3f;

        //now draw each side box

        #region Draw Boxes

        if (CheckCeil) {
            center.x = boxCenter.x;
            center.y = boxCenter.y + boxSize.y / 2f + TopThickness / 2f + TopDisplacement;

            size.x = boxSize.x * TopCoverage;
            size.y = TopThickness;

            Gizmos.color = fill;
            Gizmos.DrawCube(center, size);
            Gizmos.color = outline;
            Gizmos.DrawWireCube(center, size);
        }

        if (CheckGround) {
            center.x = boxCenter.x;
            center.y = boxCenter.y - boxSize.y / 2f - BottomThickness / 2f - BottomDisplacement;

            size.x = boxSize.x * BottomCoverage;
            size.y = BottomThickness;

            Gizmos.color = fill;
            Gizmos.DrawCube(center, size);
            Gizmos.color = outline;
            Gizmos.DrawWireCube(center, size);
        }

        if (CheckLeft) {
            center.x = boxCenter.x - boxSize.x / 2f - LeftThickness / 2f - LeftDisplacement;
            center.y = boxCenter.y;

            size.x = LeftThickness;
            size.y = boxSize.y * LeftCoverage;

            Gizmos.color = fill;
            Gizmos.DrawCube(center, size);
            Gizmos.color = outline;
            Gizmos.DrawWireCube(center, size);
        }

        if (CheckRight) {
            center.x = boxCenter.x + boxSize.x / 2f + RightThickness / 2f + RightDisplacement;
            center.y = boxCenter.y;

            size.x = RightThickness;
            size.y = boxSize.y * RightCoverage;

            Gizmos.color = fill;
            Gizmos.DrawCube(center, size);
            Gizmos.color = outline;
            Gizmos.DrawWireCube(center, size);
        }

        #endregion

    }

    public override string ToString() {
        return "(Top: " + IsCeiling + ") (Bottom: " + IsGrounded + ") (Left: " + IsLeft + ") (Right " + IsRight + ")";
    }
}
