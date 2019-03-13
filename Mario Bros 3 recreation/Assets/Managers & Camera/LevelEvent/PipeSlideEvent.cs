using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSlideEvent : LevelEvent {
    public PipeDirection pipeDirection;
    public Vector2 endLocation;
    public string endCameraBounds;
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" && Player.instance.CanEnterPipe(pipeDirection)) {
            Player.instance.transform.position = endLocation;
            LMTools.inst.SetCamBounds(endCameraBounds);
        }
    }
}
