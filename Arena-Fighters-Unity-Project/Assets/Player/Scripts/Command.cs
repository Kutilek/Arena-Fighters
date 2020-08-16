using System;
using UnityEngine;

[Serializable]
public struct Command
{
    public KeyCode keyCode;
    public bool released;
    
    public Command(KeyCode keyCode, bool released)
    {
        this.keyCode = keyCode;
        this.released = released;
    }
}
