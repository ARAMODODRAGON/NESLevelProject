using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    Player player;

    private enum powerups { dead, small, big, fire, leaf, frog };
    private powerups curPowerUp;

    private void Start() {
        player = GetComponent<Player>();

    }

    private void Update() {
        //update variables
    }

    private void LateUpdate() {
        
    }
}
