using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomoBehaviour : MonoBehaviour {
    private void FixedUpdate() {
        PhysUpdate();
        LatePhysUpdate();
    }

    /// Theses are my fixed update functions
    /// this allows me to have a late fixed update function
    protected virtual void PhysUpdate() { }
    protected virtual void LatePhysUpdate() { }
}
