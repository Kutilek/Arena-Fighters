using UnityEngine;

public class Command_Handler : MonoBehaviour
{
    private Command walkForwardCommand = new Command(KeyCode.W);
    private Command walkBackwardCommand = new Command(KeyCode.S);
    private Command walkLeftCommand = new Command(KeyCode.A);
    private Command walkRightCommand = new Command(KeyCode.D);
    private HelperCommand shiftHelperCommand = new HelperCommand(KeyCode.LeftShift);
    private HelperCommand controlHelperCommand = new HelperCommand(KeyCode.LeftControl);

    private Player_Character_Controller playerCharacterController;
    public Command currentMainCommand;
    public HelperCommand currentHelperCommand;

    void Start()
    {
        playerCharacterController = GetComponent<Player_Character_Controller>();
    }

    void Update()
    {
        if(currentMainCommand.Equals(walkForwardCommand))
        {
            playerCharacterController.WalkForward();
        }

        if(currentMainCommand.Equals(walkBackwardCommand))
        {
            playerCharacterController.WalkBackward();
        }

        if(currentMainCommand.Equals(walkLeftCommand))
        {
            playerCharacterController.WalkLeft();
        }

        if(currentMainCommand.Equals(walkRightCommand))
        {
            playerCharacterController.WalkRight();
        }

        if(currentHelperCommand.Equals(shiftHelperCommand) && currentMainCommand.Equals(walkForwardCommand))
        {
            playerCharacterController.WalkFastForward();
        }
    }
}
