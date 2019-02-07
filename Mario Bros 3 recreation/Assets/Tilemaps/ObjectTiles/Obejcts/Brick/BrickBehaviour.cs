using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour {
    public bool drawBoxes;
    public LayerMask lm;
    public enum Result { Break, Pop }
    public Result result;
    public GameObject collectable;
    public GameObject poppedObj;

    private void Start() {
        drawBoxes = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag.Equals("Player") && Player.instance.YVel >= 0.0f) {
            if (result == Result.Break) {

            } else {
                GameObject go = Instantiate(collectable, transform.position, Quaternion.identity);
                go.GetComponent<Collectable>().goUp = true;
                Instantiate(poppedObj, transform.position, Quaternion.identity);
                Destroy(gameObject);
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
