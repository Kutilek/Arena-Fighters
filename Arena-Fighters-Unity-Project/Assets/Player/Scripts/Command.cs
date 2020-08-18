using System;
using UnityEngine;

[Serializable]
public struct Command
{
    public KeyCode keyCode;
    public bool released;
    public bool doublePressed;
    public bool helperKeyPressed;
    
    public Command(KeyCode keyCode, bool released, bool doublePressed, bool helperKeyPressed)
    {
        this.keyCode = keyCode;
        this.released = released;
        this.doublePressed = doublePressed;
        this.helperKeyPressed = helperKeyPressed;
    }
}
