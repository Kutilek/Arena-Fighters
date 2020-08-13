using UnityEngine;

public class Player_Input_Handler : MonoBehaviour
{
    private KeyCode[] inputKeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E, KeyCode.F, KeyCode.Space,
    KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2};
    private Command_Handler commandHandler;
    private Command command;

    void Start()
    {
        commandHandler = GetComponent<Command_Handler>();
    }

    void Update()
    {
        GetKeyCodePressedAndReleased();
        GetMouseScrollDeltaY();
        commandHandler.currentCommand = command;
    }

    void GetKeyCodePressedAndReleased()
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if(Input.GetKeyDown(inputKey))
            {
                command.keyCode = inputKey;
            }
            else if(Input.GetKeyUp(inputKey))
            {         
                command.keyCode = KeyCode.None;
            }
        }
    }

    void GetMouseScrollDeltaY()
    {
        string message = "Mouse wheel scroll ";
        if(Input.mouseScrollDelta.y == 1.0f)
            Debug.Log(message + "up");
        else if(Input.mouseScrollDelta.y == -1.0f)
            Debug.Log(message + "down");    
    }
}
