using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {
    private Player ps;
    private Animator anim;
    private int lastPow;

    private readonly float dw = 0.9f;

    private void Start() {
        ps = GetComponent<Player>();
        anim = GetComponent<Animator>();
        for (int i = 1; i < anim.layerCount; i++) {
            anim.SetLayerWeight(i, 0.0f);
        }
        anim.SetLayerWeight(ps.currentPow, dw);
    }

    private void Update() {
        int curPow = ps.currentPow;

        if (curPow != lastPow) {
            //first set all layer's weight to 0
            for (int i = 1; i < anim.layerCount; i++) {
                anim.SetLayerWeight(i, 0.0f);
            }

            //then set the apropriate layer to a weight of 1
            switch (curPow) {
                case 1: {
                    anim.SetLayerWeight(1, dw);
                    break;
                }
                case 2: {
                    anim.SetLayerWeight(2, dw);
                    break;
                }
                case 3: {
                    anim.SetLayerWeight(3, dw);
                    break;
                }
                case 4: {
                    anim.SetLayerWeight(4, dw);
                    break;
                }
                case 5: {
                    anim.SetLayerWeight(5, dw);
                    break;
                }
                default:
                    break;
            }
        }

        lastPow = curPow;
    }
}
