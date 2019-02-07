using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick {
    // holds a string for the name of every input to get
    private string[] AllXInputs;
    private string[] AllYInputs;

    private float Deadzone;

    public Joystick(string XInput, string YInput, float deadzone_) {
        //sets the names to the first (and only) slot in both arrays
        AllXInputs = new string[1];
        AllXInputs[0] = XInput;
        AllYInputs = new string[1];
        AllYInputs[0] = YInput;
        //set deadzone 
        Deadzone = deadzone_;
    }

    public Joystick(string[] XInputs, string[] YInputs, float deadzone_) {
        //sets the arrays according to whichever is smaller
        //if one is smaller then both arrays will be set to that side
        //this avoids getting an index that doesnt exist in one array
        if (XInputs.Length > YInputs.Length) {
            AllXInputs = new string[YInputs.Length];
            AllYInputs = new string[YInputs.Length];
        } else {
            AllXInputs = new string[XInputs.Length];
            AllYInputs = new string[XInputs.Length];
        }
        //loop through the arrays and add each index to the members
        for (int i = 0; i < XInputs.Length; i++) {
            AllXInputs[i] = XInputs[i];
            AllYInputs[i] = YInputs[i];
        }
        //set deadzone 
        Deadzone = deadzone_;
    }

    //two constructor with default values for the deadzone
    public Joystick(string XInput, string YInput) : this(XInput, YInput, 0.1f) { }
    public Joystick(string[] XInputs, string[] YInputs) : this(XInputs, YInputs, 0.1f) { }
    
    public Vector2 Axis {
        get {
            //the variable to return
            Vector2 axis = Vector2.zero;
            //loops through all Inputs to see which one will be used
            for (int i = 0; i < AllXInputs.Length; i++) {
                //the absolute values are gotten in this situation so i only have to compare if its above a value
                float x = Mathf.Abs(Input.GetAxisRaw(AllXInputs[i]));
                float y = Mathf.Abs(Input.GetAxisRaw(AllYInputs[i]));

                if (x >= Deadzone) {
                    axis.x = Input.GetAxisRaw(AllXInputs[i]);
                    axis.y = Input.GetAxisRaw(AllYInputs[i]);
                    break;
                } else if (y >= Deadzone) {
                    axis.x = Input.GetAxisRaw(AllXInputs[i]);
                    axis.y = Input.GetAxisRaw(AllYInputs[i]);
                    break;
                }
            }
            return axis;
        }
    }

    public void getDir() {
        if (Axis.x < 0.0f) Left = true;
        else Left = false;

        if (Axis.x > 0.0f) Right = true;
        else Right = false;
        
        if (Axis.y < 0.0f) Down = true;
        else Down = false;

        if (Axis.y > 0.0f) Up = true;
        else Up = false;
    }

    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool Down { get; private set; }
    public bool Up { get; private set; }
}
