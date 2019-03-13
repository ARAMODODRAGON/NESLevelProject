using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PipeDirection {
    up,
    down,
    left,
    right
}

public class PipeEvent : LevelEvent {
    public PipeDirection pipeDirection;
    public int level;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" && Player.instance.CanEnterPipe(pipeDirection)) {
            GameManager.LoadLevel(level);
        }
    }
}
