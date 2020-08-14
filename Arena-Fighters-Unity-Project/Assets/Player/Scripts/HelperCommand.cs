using System;
using UnityEngine;

[Serializable]
public struct HelperCommand
{
    public KeyCode keyCode;
    
    public HelperCommand(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
}
