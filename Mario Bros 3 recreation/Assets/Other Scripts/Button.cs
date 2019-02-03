using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button {
    // holds a string for the name of every input to get
    private string[] AllInputs;
    private bool lastCheck;

    public bool ButtonHeld { get; private set; }
    public bool ButtonDown { get; private set; }
    public bool ButtonUp { get; private set; }

    public Button(string Input) {
        //sets the names to the first (and only) slot in both arrays
        AllInputs = new string[1];
        AllInputs[1] = Input;
    }

    public Button(string[] Inputs) {
        //copies the array into AllInputs
        AllInputs = Inputs;
    }
    
    public void getInput() {
        ButtonHeld = false;
        //gets the values of all buttons
        foreach (string bName in AllInputs) {
            ButtonHeld |= (Input.GetAxis(bName) != 0.0f);
        }

        if (lastCheck && !ButtonHeld) ButtonUp = true;
        else ButtonUp = false;

        if (!lastCheck && ButtonHeld) ButtonDown = true;
        else ButtonDown = false;

        lastCheck = ButtonHeld;
    }


}
