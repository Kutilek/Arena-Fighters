using UnityEngine;

public class Player_Input_Handler : MonoBehaviour
{
    private KeyCode[] mainInputKeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, 
    KeyCode.E, KeyCode.F, KeyCode.Space, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2};
    private KeyCode[] helperInputKeys = {KeyCode.LeftShift, KeyCode.LeftControl};
    private Command_Handler commandHandler;

    void Start()
    {
        commandHandler = GetComponent<Command_Handler>();
    }

    void Update()
    {
        GetMouseScrollDeltaY();
        commandHandler.currentMainCommand = CreateCommand(mainInputKeys);
        commandHandler.currentHelperCommand = CreateHelperCommand(helperInputKeys);
    }

    Command CreateCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyDown(inputKey))
                return new Command(inputKey, false);
            if (Input.GetKeyUp(inputKey))
                return new Command(inputKey, true); 
        }
        return new Command();
    }

    HelperCommand CreateHelperCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKey(inputKey))
                return new HelperCommand(inputKey);
        }
        return new HelperCommand();
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
