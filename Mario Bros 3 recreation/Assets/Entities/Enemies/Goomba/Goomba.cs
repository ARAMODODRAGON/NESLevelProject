using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemies {
    protected override void OnScreen() {

    }

    public override void TakeDamage() {
        Debug.Log("KILL CONFIRMED");
    }
}
