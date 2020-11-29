using UnityEngine;

public struct InputCommand
{
    public KeyCode keyCode;
    public bool released;
    public bool doublePressed;
    
    public InputCommand(KeyCode keyCode, bool released, bool doublePressed)
    {
        this.keyCode = keyCode;
        this.released = released;
        this.doublePressed = doublePressed;
    }
}
