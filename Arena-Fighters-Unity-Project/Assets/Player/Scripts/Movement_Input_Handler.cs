using UnityEngine;

public class Movement_Input_Handler : Input_Handler
{
    private KeyCode lastPressed;
    private float doublePressTimer;

    public float doublePressTime;

    protected override void Start()
    {
        base.Start();
        helperKey = KeyCode.LeftShift;
        inputKeys = new KeyCode[5] {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
    }

    void Update()
    {
        playerController.moveHelperKeyPressed = GetHelperKeyPressed();
        playerController.direction = GetMoveDirection();
    }

    Vector3 GetMoveDirection()
    {
        float ad = Input.GetAxisRaw("Horizontal");
        float ws = Input.GetAxisRaw("Vertical");
        return new Vector3(ad, 0f, ws).normalized;
    }
    
/*
    protected override Command CreateCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if(Input.GetKeyDown(inputKey) && lastPressed == inputKey) 
            {
                lastPressed = KeyCode.None;
                if(Time.time - doublePressTimer < doublePressTime) 
                {
                    doublePressTimer = 0f;
                    return new Command(inputKey, false, true, helperKeyPressed);
                }
            }

            if (Input.GetKey(inputKey))
                return new Command(inputKey, false, false, helperKeyPressed);

            if (Input.GetKeyUp(inputKey))
            {
                lastPressed = inputKey;
                doublePressTimer = Time.time;
                return new Command(inputKey, true, false, helperKeyPressed);
            }
        }
        return new Command();
    }*/
}
