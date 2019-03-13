using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitBehaviour : MonoBehaviour {
    public int direction = 1;
    private float timer = 0f;

    private SpriteRenderer sp;

    private void Start() {
        switch (direction) {
            case 1:
                GetComponent<Rigidbody2D>().velocity = new Vector2(2f, 5f);
                break;
            case 2:
                GetComponent<Rigidbody2D>().velocity = new Vector2(-2f, 5f);
                break;
            case 3:
                GetComponent<Rigidbody2D>().velocity = new Vector2(-2f, 3f);
                break;
            case 4:
                GetComponent<Rigidbody2D>().velocity = new Vector2(2f, 3f);
                break;
            default:
                goto case 1;
        }
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer > 0.25f) {
            timer = 0f;
            sp.flipX = !sp.flipX;
        }
    }
}
