using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Entity {

    [Header("Water Movement")]
    public float xMaxWaterSpeed;
    public float xMaxGroundedSpeed;
    public float xWaterAccel;
    public float xWaterDecel;
    public float MaxWaterRiseSpeed;
    public float MaxWaterFallSpeed;

    private void WaterUpdate() {
        HandleYWaterMovement();
    }

    private void HandleYWaterMovement() {

    }


}
