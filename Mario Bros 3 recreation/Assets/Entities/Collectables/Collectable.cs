using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : Entity {

    protected abstract override void OnScreen();

    protected override void OffScreen() {
        //delete the PowerUp
        //this is overriden by the coin as coins stay existant when off screen
        Destroy(gameObject);
    }
    
}
