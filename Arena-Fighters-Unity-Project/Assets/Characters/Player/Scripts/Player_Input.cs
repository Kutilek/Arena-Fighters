using UnityEngine;

[RequireComponent(typeof(Player_Physics))]
public class Player_Input : MonoBehaviour
{
    // Components
    private Player_Physics characterPhysics;
    private Wall_Interaction wallInteraction;
    private Dash dash;
    private Jump jump;
    private Light_Attack lightAttack;
    private Stun_Target stunTarget;
    private Immobilize_Target immobilizeTarget;

    #region Inputs / Commands

    // Input Keys
    private KeyCode[] inputKeys = {KeyCode.Mouse0, KeyCode.E, KeyCode.Q, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};

    // Current Movement Inputs
    private InputCommand pressCommand;
    private InputCommand doublePressCommand;

    // Combat Commands
    private InputCommand lightAttackCommand = new InputCommand(KeyCode.Mouse0, false, false);
    private InputCommand stunTargetCommand = new InputCommand(KeyCode.E, false, false);
    private InputCommand immobilizeTargetCommand = new InputCommand(KeyCode.Q, false, false);

    // Movement Commands 
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
        lightAttack = GetComponent<Light_Attack>();
        stunTarget = GetComponent<Stun_Target>();
        immobilizeTarget = GetComponent<Immobilize_Target>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        characterPhysics.inputDirectionRaw = GetMoveDirection();
        pressCommand = CreatePressCommand(inputKeys);

        if (characterPhysics.currentGravityState == GravityState.OnWall)
        {
            // Need to calculate direction on wall differently
            characterPhysics.inputDirection = wallInteraction.CalculateDirectiOnOnWall(characterPhysics.inputDirectionRaw);

            if (pressCommand.Equals(jumpCommand))
                wallInteraction.JumpOffWall();
        }
        else
        {
            doublePressCommand = CreateDoublePressCommand(inputKeys);

            // Check for Dash Command
            if (doublePressCommand.Equals(dashBackwardCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);
            else if (doublePressCommand.Equals(dashForwardCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);
            else if (doublePressCommand.Equals(dashRightCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);
            else if (doublePressCommand.Equals(dashLeftCommand))
                dash.CastDash(characterPhysics.inputDirectionRaw);

            if (pressCommand.Equals(jumpCommand))
                jump.CastJump();

            if (pressCommand.Equals(lightAttackCommand))
                lightAttack.Cast();

            if (pressCommand.Equals(stunTargetCommand))
                stunTarget.Cast();

            if (pressCommand.Equals(immobilizeTargetCommand))
                immobilizeTarget.Cast();
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
