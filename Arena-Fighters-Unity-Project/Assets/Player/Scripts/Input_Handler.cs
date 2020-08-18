using UnityEngine;

public abstract class Input_Handler : MonoBehaviour
{
    protected Player_Controller commandHandler;
    protected bool helperKeyPressed;
    protected KeyCode[] inputKeys;
    protected KeyCode helperKey;
    
    protected virtual void Start()
    {
        commandHandler = GetComponent<Player_Controller>();
    }

    protected void GetHelperKeyPressed()
    {
        if (Input.GetKey(helperKey))
            helperKeyPressed = true;
        else
            helperKeyPressed = false;
    }

    protected virtual Command CreateCommand(KeyCode[] inputKeys)
    {
        return new Command();
    }
}
