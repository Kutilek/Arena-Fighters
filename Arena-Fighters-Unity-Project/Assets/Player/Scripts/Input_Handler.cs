using UnityEngine;

public abstract class Input_Handler : MonoBehaviour
{
    protected Player_Controller playerController;
    protected KeyCode[] inputKeys;
    protected KeyCode helperKey;
    
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

    protected virtual Command CreateCommand(KeyCode[] inputKeys)
    {
        return new Command();
    }
}
