using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {
    public float flashTrasitionTime;
    public int numberOfFlashes;

    private Player ps;
    private Animator anim;
    private Rigidbody2D rb;
    private int lastPow;

    private void Start() {
        ps = GetComponent<Player>();
        lastPow = ps.CurrentPow;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        for (int i = 1; i < anim.layerCount; i++) {
            anim.SetLayerWeight(i, 0.0f);
        }
        anim.SetLayerWeight(ps.CurrentPow, 1.0f);
    }

    private void Update() {
        int curPow = ps.CurrentPow;

        if (curPow != lastPow) {
            //first set all layer's weight to 0
            for (int i = 1; i < anim.layerCount; i++) {
                anim.SetLayerWeight(i, 0.0f);
            }

            //then set the apropriate layer to a weight of 1
            switch (curPow) {
                case 0: {
                    StartCoroutine(DeathAnimation());
                    break;
                }
                case 1: {
                    StartCoroutine(FlashTransition(curPow));
                    break;
                }
                case 2: {
                    StartCoroutine(FlashTransition(curPow));
                    break;
                }
                case 3: {
                    StartCoroutine(FlashTransition(curPow));
                    break;
                }
                case 4: {
                    StartCoroutine(FlashTransition(curPow));
                    break;
                }
                case 5: {
                    StartCoroutine(FlashTransition(curPow));
                    break;
                }
                default:
                    break;
            }
        }
    }

    IEnumerator FlashTransition(int ToThislayer) {
        ps.isTransitioning = true;

        for (int i = 0; i <= numberOfFlashes; i++) {
            anim.SetLayerWeight(lastPow, 1.0f);
            anim.SetLayerWeight(ToThislayer, 0.0f);
            yield return new WaitForSeconds(flashTrasitionTime);

            anim.SetLayerWeight(lastPow, 0.0f);
            anim.SetLayerWeight(ToThislayer, 1.0f);
            if (i != numberOfFlashes) yield return new WaitForSeconds(flashTrasitionTime);
        }

        ps.isTransitioning = false;

        lastPow = ToThislayer;
    }

    IEnumerator DeathAnimation() {
        lastPow = 0;
        foreach (BoxCollider2D col in GetComponents<BoxCollider2D>()) {
            col.isTrigger = true;
        }
        anim.SetLayerWeight(11, 1f);
        rb.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0f, 8f);
        while (rb.velocity.y > -8) {
            Vector2 newVel = rb.velocity;
            newVel.y -= Time.fixedDeltaTime * 16f;
            rb.velocity = newVel;
            yield return new WaitForFixedUpdate();
        }
    }
}
