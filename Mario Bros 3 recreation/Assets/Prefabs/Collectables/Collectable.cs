using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Entity {
    protected override void Start() {
        base.Start();
    }

    protected override void OnScreen() {
        //do nothing
    }

    protected override void OffScreen() {
        //delete the PowerUp
        //this is overriden by the coin as coins stay existant
        Destroy(this);
    }
    
}
