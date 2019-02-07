using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlower : Collectable {
    protected override void Start() {
        base.Start();
        itemName = "FireFlower";
    }

    protected override void OnScreen() {
        base.OnScreen();
    }
}
