using UnityEngine;

public class Command_Handler : MonoBehaviour
{
    private Command walkForwardCommand = new Command(KeyCode.W, false);
    private Command walkBackwardCommand = new Command(KeyCode.S, false);
    private Command walkLeftCommand = new Command(KeyCode.A, false);
    private Command walkRightCommand = new Command(KeyCode.D, false);
    private Command walkForwardRelease = new Command(KeyCode.W, true);
    private Command walkBackwardRelease = new Command(KeyCode.S, true);
    private Command walkLeftRelease = new Command(KeyCode.A, true);
    private Command walkRightRelease = new Command(KeyCode.D, true);
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
        if (currentMainCommand.Equals(walkForwardCommand))
        {
            playerCharacterController.walkingForward = true;
        }
        else if (currentMainCommand.Equals(walkBackwardCommand))
        {
            playerCharacterController.walkingBackward = true;
        }
        else if (currentMainCommand.Equals(walkLeftCommand))
        {
            playerCharacterController.walkingLeft = true;
        }
        else if (currentMainCommand.Equals(walkRightCommand))
        {
            playerCharacterController.walkingRight = true;
        }

        else if (currentMainCommand.Equals(walkForwardRelease))
        {
            playerCharacterController.walkingForward = false;
        }
        else if (currentMainCommand.Equals(walkBackwardRelease))
        {
            playerCharacterController.walkingBackward = false;
        }
        else if (currentMainCommand.Equals(walkLeftRelease))
        {
            playerCharacterController.walkingLeft = false;
        }
        else if (currentMainCommand.Equals(walkRightRelease))
        {
            playerCharacterController.walkingRight = false;
        }

        GetHelperCommand();
    }

    void GetHelperCommand()
    {
        if (currentHelperCommand.Equals(shiftHelperCommand))
            playerCharacterController.shiftHelperKey = true;
        else if (currentHelperCommand.Equals(controlHelperCommand))
        {

        }
        else
        {
            playerCharacterController.shiftHelperKey = false;
        }
    }
}
