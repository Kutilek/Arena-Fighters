using UnityEngine;

public abstract class Input_Handler : MonoBehaviour
{
    protected Player_Controller playerController;
    protected KeyCode[] inputKeys;
    protected KeyCode helperKey;
    private KeyCode lastPressed;
    private float doublePressTimer;
    [SerializeField] private float doublePressTime;
    
    protected virtual void Start()
    {
        playerController = GetComponent<Player_Controller>();
    }

    protected bool GetHelperKeyPressed()
    {
        if (Input.GetKey(helperKey))
            return true;
        else
            return false;
    }

    protected virtual Command CreatePressCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyDown(inputKey))
                return new Command(inputKey, false, false);
        }
        return new Command();
    }

    protected virtual Command CreateDoublePressCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyDown(inputKey))
            {
                if (lastPressed == inputKey)
                {
                    lastPressed = KeyCode.None;
                    if (Time.time - doublePressTimer < doublePressTime)
                        return new Command(inputKey, false, true);
                }
                lastPressed = inputKey;
                doublePressTimer = Time.time;
            }
        }
        return new Command();
    }
}
