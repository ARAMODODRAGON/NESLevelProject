using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * created by Domara Shlimon
 * ver 4.0
 */

public class ChildTrigger : MonoBehaviour {
    MultiTrigger mt;
    [HideInInspector]
    public LayerMask lm;
    private int c;

    private void Awake() {
        c = 0;
        mt = GetComponentInParent<MultiTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.IsTouchingLayers(lm)) {
            c++;
            mt.setTrigger(true, name);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        if (col.IsTouchingLayers(lm)) {
            c--;
            if (c == 0) mt.setTrigger(false, name);
        }
    }
    
}
