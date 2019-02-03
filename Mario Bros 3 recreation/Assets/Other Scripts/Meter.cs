using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter {
    private float min;
    private float max;
    private float amount;

    public Meter(float min_, float max_) {
        min = min_;
        max = max_;
        amount = min;
    }

    public Meter(float Range) {
        min = -Range;
        max = Range;
        amount = 0.0f;
    }

    public float Range {
        get {
            if (-min == max) return max;
            else return 0.0f;
        }
        set {
            min = -value;
            max = value;
        }
    }
    public float Min {
        get {
            return min;
        }
        set {
            if (value < max) { min = value; }
        }
    }
    public float Max {
        get {
            return max;
        }
        set {
            if (value > min) { max = value; }
        }
    }

    public float Amount {
        get { return amount; }
        set {
            //sets the count but checks if it is within the
            //min and max values and ajusts it for that 
            bool withinBounds = false;
            if (amount >= min && amount <= max) withinBounds = true;
            
            amount = value;

            if (amount < min && withinBounds) amount = min;
            if (amount > max && withinBounds) amount = max;
        }
    }

    public float Abs {
        get {
            if (-min == max) return Mathf.Abs(amount); 
            else return 0.0f;
        }
    }
}
