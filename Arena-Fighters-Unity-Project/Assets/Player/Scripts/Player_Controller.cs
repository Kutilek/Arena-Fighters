using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    #region Movement Commands

    // Forward Movement Commands
    private Command dashForward = new Command(KeyCode.W, false, true, false);
    private Command dashForwardSpecial = new Command(KeyCode.W, false, true, true);

    // Left Movement Commands
    private Command dashLeft = new Command(KeyCode.A, false, true, false);
    private Command dashLeftSpecial = new Command(KeyCode.A, false, true, true);

    // Backward Movement Commands
    private Command dashBackward = new Command(KeyCode.S, false, true, false);
    private Command dashBackwardSpecial = new Command(KeyCode.S, false, true, true);

    // Right Movement Commands
    private Command dashRight = new Command(KeyCode.D, false, true, false);
    private Command dashRightSpecial = new Command(KeyCode.D, false, true, true);

    // Jump Movement Commands
    private Command jump = new Command(KeyCode.Space, false, false, false);

    public Command movementCommand;

    #endregion

    private Character_Behaviour characterBehaviour;
    public bool moveHelperKeyPressed;
    public Vector3 direction;

    void Start()
    {
        characterBehaviour = GetComponent<Character_Behaviour>();
    }

    void Update()
    {
        if (direction.magnitude >= 0.1f)
        {
            if (moveHelperKeyPressed)
            {
                characterBehaviour.Run(direction);
            }
            characterBehaviour.Walk(direction);
        }

        /*
        // Forward Movement
        if(movementCommand.Equals(walkForward))
        {
            characterBehaviour.WalkForward();
        }
        else if(movementCommand.Equals(dashForward))
        {
            Debug.Log("I am dashing forward");
        }
        else if(movementCommand.Equals(runForward))
        {
            Debug.Log("I am running forward!");
        }
        else if(movementCommand.Equals(dashForwardSpecial))
        {
            Debug.Log("I am having a longer forward dash!");
        }
        // Left Movement
        else if(movementCommand.Equals(walkLeft))
        {
            characterBehaviour.WalkLeft();
        }
        else if(movementCommand.Equals(dashLeft))
        {
            Debug.Log("I am dashing left");
        }
        else if(movementCommand.Equals(runLeft))
        {
            Debug.Log("I am running left!");
        }
        else if(movementCommand.Equals(dashLeftSpecial))
        {
            Debug.Log("I am having a longer left dash!");
        }
        // Backward Movement
        else if(movementCommand.Equals(walkBackward))
        {
            characterBehaviour.WalkBack();
        }
        else if(movementCommand.Equals(dashBackward))
        {
            Debug.Log("I am dashing Backward");
        }
        else if(movementCommand.Equals(runBackward))
        {
            Debug.Log("I am running Backward!");
        }
        else if(movementCommand.Equals(dashBackwardSpecial))
        {
            Debug.Log("I am having a longer Backward dash!");
        }
        // Right Movement
        else if(movementCommand.Equals(walkRight))
        {
            characterBehaviour.WalkRight();
        }
        else if(movementCommand.Equals(dashRight))
        {
            Debug.Log("I am dashing Right");
        }
        else if(movementCommand.Equals(runRight))
        {
            Debug.Log("I am running Right!");
        }
        else if(movementCommand.Equals(dashRightSpecial))
        {
            Debug.Log("I am having a longer Right dash!");
        }

        else if(movementCommand.Equals(jump))
        {
            Debug.Log("I am jumping!");
        }*/
    }
}
