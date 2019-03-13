using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollCheck : MonoBehaviour {
    [Header("Accuracy of Check")]
    [Range(0.1f, 0.9f)]
    public float accur = 0.5f;
    
    public bool IsColl { get; private set; }
    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool IsCeiling { get; private set; }
    public bool IsGrounded { get; private set; }

    private List<ContactPoint2D> conp = new List<ContactPoint2D>();

    private void Start() {
        StartCoroutine(Collision());
    }

    private void OnCollisionStay2D(Collision2D col) {
        for (int i = 0; i < col.contactCount; i++) {
            conp.Add(col.GetContact(i));
        }
    }

    private IEnumerator Collision() {
        while (true) {
            yield return new WaitForFixedUpdate();

            Left = false;
            Right = false;
            IsGrounded = false;
            IsCeiling = false;

            while (conp.Count > 0) {
                //IsGrounded
                if (Vector3.Dot(conp[0].normal, Vector3.up) > accur) {
                    IsGrounded |= true;
                }

                //Left
                if (Vector3.Dot(conp[0].normal, Vector3.right) > accur) {
                    Left |= true;
                }

                //Right
                if (Vector3.Dot(conp[0].normal, Vector3.left) > accur) {
                    Right |= true;
                }

                //IsCeiling
                if (Vector3.Dot(conp[0].normal, Vector3.down) > accur) {
                    IsCeiling |= true;
                }

                conp.RemoveAt(conp.Count - 1);
            }

            IsColl = Left || Right || IsGrounded || IsCeiling;
        }
    }

    public override string ToString() {
        return "IsColl=" + IsColl + ", Left=" + Left + ", Right=" + Right +
            ", Top=" + IsCeiling + ", Ground=" + IsGrounded;
    }
}


/*
Debug.DrawRay(point[0], Vector2.up * 0.05f, Color.red, 0.1f);
Debug.DrawRay(point[0], Vector2.left * 0.05f, Color.red, 0.1f);
Debug.DrawRay(point[0], Vector2.down * 0.05f, Color.red, 0.1f);
Debug.DrawRay(point[0], Vector2.right * 0.05f, Color.red, 0.1f);

Debug.DrawRay(point[1], Vector2.up * 0.05f, Color.red, 0.1f);
Debug.DrawRay(point[1], Vector2.left * 0.05f, Color.red, 0.1f);
Debug.DrawRay(point[1], Vector2.down * 0.05f, Color.red, 0.1f);
Debug.DrawRay(point[1], Vector2.right * 0.05f, Color.red, 0.1f);



    Collider2D c = collision.collider;
    Vector2[] point = new Vector2[2];
    point[0] = col.bounds.ClosestPoint(c.bounds.min);
    point[1] = col.bounds.ClosestPoint(c.bounds.max);

    #region 2 point check
    //left
    if (point[0].x <= col.bounds.min.x && point[1].x <= col.bounds.min.x) {
        Left = true;
        DrawPoints(point, Color.blue);
    }

    //right
    if (point[0].x >= col.bounds.max.x && point[1].x >= col.bounds.max.x) {
        Right = true;
    }

    //ground
    if (point[0].y <= col.bounds.min.y && point[1].y <= col.bounds.min.y) {
        IsGrounded = true;
    }

    //top
    if (point[0].y >= col.bounds.max.y && point[1].y >= col.bounds.max.y) {
        IsCeiling = true;
    }
    #endregion

    #region old 1 point check
    /*
    foreach (var p in point) {
        if (p.x <= col.bounds.min.x && p.y >= col.bounds.min.y && p.y <= col.bounds.max.y) {
            Left = true;
        }

        if (p.x >= col.bounds.max.x && p.y >= col.bounds.min.y && p.y <= col.bounds.max.y) {
            Right = true;
        }

        if (p.y <= col.bounds.min.y && p.x >= col.bounds.min.x && p.x <= col.bounds.max.x) {
            IsGrounded = true;
        }

        if (p.y >= col.bounds.max.y && p.x >= col.bounds.min.x && p.x <= col.bounds.max.x) {
            IsCeiling = true;
        }
    }
     //
    #endregion
    
    private void DrawPoints(Vector2[] points, Color color) {
        foreach (Vector2 point in points) {
            Debug.DrawRay(point, Vector2.up * 0.05f, color, 2.0f);
            Debug.DrawRay(point, Vector2.left * 0.05f, color, 2.0f);
            Debug.DrawRay(point, Vector2.down * 0.05f, color, 2.0f);
            Debug.DrawRay(point, Vector2.right * 0.05f, color, 2.0f);
        }
    }

    private void DrawPoint(Vector2 point, Color color) {
        Debug.DrawRay(point, Vector2.up * 0.05f, color, 0.1f);
        Debug.DrawRay(point, Vector2.left * 0.05f, color, 0.1f);
        Debug.DrawRay(point, Vector2.down * 0.05f, color, 0.1f);
        Debug.DrawRay(point, Vector2.right * 0.05f, color, 0.1f);
    }
    
    private void DrawPoint(Vector2 point, Color color) {
        Debug.DrawRay(point, Vector2.up * 0.05f, color, 10.0f);
        Debug.DrawRay(point, Vector2.left * 0.05f, color, 10.0f);
        Debug.DrawRay(point, Vector2.down * 0.05f, color, 10.0f);
        Debug.DrawRay(point, Vector2.right * 0.05f, color, 10.0f);
    }
    */
