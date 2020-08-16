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
            playerCharacterController.walkingForward = true;
        }

        else if(currentMainCommand.Equals(walkBackwardCommand))
        {
            playerCharacterController.walkingBackward = true;
        }

        else if(currentMainCommand.Equals(walkLeftCommand))
        {
            playerCharacterController.walkingLeft = true;
        }

        else if(currentMainCommand.Equals(walkRightCommand))
        {
            playerCharacterController.walkingRight = true;
        }

        else if(currentHelperCommand.Equals(shiftHelperCommand))
        {
            playerCharacterController.shiftHelperKey = true;
        }
    }
}
