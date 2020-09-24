using UnityEngine;

public class Attack_Input_Handler : Input_Handler
{
    protected override void Start()
    {
        base.Start();
        helperKey = KeyCode.LeftControl;
        inputKeys = new KeyCode[6] {KeyCode.F, KeyCode.Q, KeyCode.E, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2};
    }

    private void Update()
    {
        playerController.attackHelperKeyPressed = GetHelperKeyPressed();
        playerController.pressAttackCommand = CreatePressCommand(inputKeys);
        playerController.releaseAttackCommand = CreateReleaseCommand(inputKeys);
        playerController.mouseScroll = Input.mouseScrollDelta.y;
    }
}
