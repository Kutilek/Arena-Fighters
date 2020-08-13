using System;
using UnityEngine;

[Serializable]
public struct Command
{
    public KeyCode keyCode;
    
    public Command(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
}
