using UnityEngine;

public class Command_Handler : MonoBehaviour
{
    private Command walkForwardCommand = new Command(KeyCode.W);
    private Command walkBackwardCommand = new Command(KeyCode.S);
    private Command walkLeftCommand = new Command(KeyCode.A);
    private Command walkRightCommand = new Command(KeyCode.D);

    private Player_Character_Controller playerCharacterController;
    public Command currentCommand;

    void Start()
    {
        playerCharacterController = GetComponent<Player_Character_Controller>();
    }

    void Update()
    {
        if(currentCommand.Equals(walkForwardCommand))
        {
            playerCharacterController.WalkForward();
        }

        if(currentCommand.Equals(walkBackwardCommand))
        {
            playerCharacterController.WalkBackward();
        }

        if(currentCommand.Equals(walkLeftCommand))
        {
            playerCharacterController.WalkLeft();
        }

        if(currentCommand.Equals(walkRightCommand))
        {
            playerCharacterController.WalkRight();
        }
    }
}
