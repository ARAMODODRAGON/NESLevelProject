using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Entity {
    protected override void Start() {
        base.Start();
    }

    protected override void OnScreen() {
        //do nothing
    }

    protected override void OffScreen() {
        //delete the powerup
        Destroy(this);
    }
}
