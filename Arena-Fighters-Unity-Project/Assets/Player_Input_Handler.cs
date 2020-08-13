using UnityEngine;

public class Player_Input_Handler : MonoBehaviour
{
    private KeyCode[] inputKeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E, KeyCode.F, KeyCode.Space,
    KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2};

    void Update()
    {
        GetKeyCodePressedAndReleased();
        GetMouseScrollDeltaY();
    }

    void GetKeyCodePressedAndReleased()
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            string message = inputKey.ToString() + " was ";
            if(Input.GetKeyDown(inputKey))
            {
                Debug.Log(message + "pressed");
            }
            else if(Input.GetKeyUp(inputKey))
            {         
                Debug.Log(message + "released");
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
