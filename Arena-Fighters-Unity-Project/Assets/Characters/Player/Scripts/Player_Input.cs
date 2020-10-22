using UnityEngine;

[RequireComponent(typeof(Player_Physics))]
public class Player_Input : MonoBehaviour
{
    private Player_Physics characterPhysics;
    private Wall_Interaction wallInteraction;
    private Dash dash;
    private Jump jump;

    #region Movement Inputs / Commands

    // Movement Input Keys
    private KeyCode[] inputKeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
    private KeyCode movementHelperKey = KeyCode.LeftShift;

    // Current Movement Inputs
    private InputCommand pressMovementCommand;
    private InputCommand doublePressMovementCommand;
    private bool moveHelperKeyPressed;

    // Commands
    private InputCommand dashForwardCommand = new InputCommand(KeyCode.W, false, true);
    private InputCommand dashLeftCommand = new InputCommand(KeyCode.A, false, true);
    private InputCommand dashBackwardCommand = new InputCommand(KeyCode.S, false, true);
    private InputCommand dashRightCommand = new InputCommand(KeyCode.D, false, true);
    private InputCommand jumpCommand = new InputCommand(KeyCode.Space, false, false);

    #endregion

    private void Awake()
    {
        if (doublePressTime == 0f)
            doublePressTime = 0.25f;
        characterPhysics = GetComponent<Player_Physics>();
        wallInteraction = GetComponent<Wall_Interaction>();
        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        characterPhysics.inputDirectionRaw = GetMoveDirection();
        pressMovementCommand = CreatePressCommand(inputKeys);

        if (characterPhysics.currentGravityState == GravityState.OnWall)
        {
            // Need to calculate direction on wall differently
            characterPhysics.inputDirection = wallInteraction.CalculateDirectiOnOnWall(characterPhysics.inputDirectionRaw);

            if (pressMovementCommand.Equals(jumpCommand))
                wallInteraction.JumpOffWall();
        }
        else
        {
            doublePressMovementCommand = CreateDoublePressCommand(inputKeys);

            // Check for Dash Command
            if (doublePressMovementCommand.Equals(dashBackwardCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);
            else if (doublePressMovementCommand.Equals(dashForwardCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);
            else if (doublePressMovementCommand.Equals(dashRightCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);
            else if (doublePressMovementCommand.Equals(dashLeftCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);

            if (pressMovementCommand.Equals(jumpCommand))
                jump.CastJump();
        }
    }

    // Get Raw Input from Movement keys
    private Vector3 GetMoveDirection()
    {
        float ad = Input.GetAxisRaw("Horizontal");
        float ws = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(ad, 0f, ws).normalized;

        return direction;
    }

    private bool GetHelperKeyPressed(KeyCode helperKey)
    {
        if (Input.GetKey(helperKey))
            return true;
        else
            return false;
    }

    #region Command Creators

    private InputCommand CreatePressCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyDown(inputKey))
                return new InputCommand(inputKey, false, false);
        }
        return new InputCommand();
    }

    private InputCommand CreateReleaseCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyUp(inputKey))
                return new InputCommand(inputKey, true, false);
        }
        return new InputCommand();
    }


    private KeyCode lastPressed;
    private float doublePressTimer;
    [SerializeField] private float doublePressTime;

    private InputCommand CreateDoublePressCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyDown(inputKey))
            {
                if (lastPressed == inputKey)
                {
                    lastPressed = KeyCode.None;
                    if (Time.time - doublePressTimer < doublePressTime)
                        return new InputCommand(inputKey, false, true);
                }
                lastPressed = inputKey;
                doublePressTimer = Time.time;
            }
        }
        return new InputCommand();
    }

    #endregion
}
