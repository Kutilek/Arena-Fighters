using UnityEngine;

public class Movement_Input_Handler : Input_Handler
{  
    protected override void Start()
    {
        base.Start();
        helperKey = KeyCode.LeftShift;
        inputKeys = new KeyCode[5] {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
    }

    void Update()
    {
        playerController.moveHelperKeyPressed = GetHelperKeyPressed();
        playerController.movementCommand = CreateDoublePressCommand(inputKeys);
        playerController.direction = GetMoveDirection();
    }

    Vector3 GetMoveDirection()
    {
        float ad = Input.GetAxisRaw("Horizontal");
        float ws = Input.GetAxisRaw("Vertical");
        return new Vector3(ad, 0f, ws).normalized;
    }
}
