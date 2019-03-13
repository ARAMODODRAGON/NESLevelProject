using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
                Instantiate(collectable, transform.position + new Vector3(-0.25f, 0.25f), Quaternion.identity)
                    .GetComponent<BitBehaviour>().direction = 1;

                Instantiate(collectable, transform.position + new Vector3(-0.25f, 0.25f), Quaternion.Euler(new Vector3(0f, 180f)))
                    .GetComponent<BitBehaviour>().direction = 2;

                Instantiate(collectable, transform.position + new Vector3(-0.25f, 0.25f), Quaternion.Euler(new Vector3(0f, 180f)))
                    .GetComponent<BitBehaviour>().direction = 3;

                Instantiate(collectable, transform.position + new Vector3(-0.25f, 0.25f), Quaternion.identity)
                    .GetComponent<BitBehaviour>().direction = 4;
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
