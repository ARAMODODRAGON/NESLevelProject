using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button {
    // holds a string for the name of every input to get
    private string[] AllInputs;

    public Button(string Input) {
        //sets the names to the first (and only) slot in both arrays
        AllInputs = new string[1];
        AllInputs[1] = Input;
    }

    public Button(string[] Inputs) {
        //copies the array into AllInputs
        AllInputs = Inputs;
    }

    public bool getButton() {
        bool result = false;
        //gets the values of all buttons
        foreach (string name in AllInputs) {
            result |= Input.GetButton(name);
        }

        return result;
    }

    public bool getButtonDown() {
        bool result = false;
        //gets the values of all buttons
        foreach (string name in AllInputs) {
            result |= Input.GetButtonDown(name);
        }

        return result;
    }

    public bool getButtonUp() {
        bool result = false;
        //gets the values of all buttons
        foreach (string name in AllInputs) {
            result |= Input.GetButtonUp(name);
        }

        return result;
    }
}
