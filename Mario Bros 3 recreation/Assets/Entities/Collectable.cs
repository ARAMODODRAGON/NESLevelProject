using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : Agent {

    protected abstract override void OnScreen();

    protected override void OffScreen() {
        //delete the PowerUp
        //this is overriden by the coin
        Destroy(gameObject);
    }
    
}
