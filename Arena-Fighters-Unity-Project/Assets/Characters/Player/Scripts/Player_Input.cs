using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
public class Player_Input : MonoBehaviour
{
    protected Character_Physics characterPhysics;
    protected Dash dash;
    protected Jump jump;
    protected Wall_Interaction wallInteraction;
    protected KeyCode[] inputKeys = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space};
    protected KeyCode movementHelperKey = KeyCode.LeftShift;
    private KeyCode lastPressed;
    private float doublePressTimer;
    [SerializeField] private float doublePressTime;

    private InputCommand dashForwardCommand = new InputCommand(KeyCode.W, false, true);
    private InputCommand dashLeftCommand = new InputCommand(KeyCode.A, false, true);
    private InputCommand dashBackwardCommand = new InputCommand(KeyCode.S, false, true);
    private InputCommand dashRightCommand = new InputCommand(KeyCode.D, false, true);
    private InputCommand jumpCommand = new InputCommand(KeyCode.Space, false, false);

    // Current Movement Inputs
    private InputCommand pressMovementCommand;
    private InputCommand doublePressMovementCommand;
    private Vector3 inputDirection;
    private bool moveHelperKeyPressed;

    // Transforms needed for player movement
    private Transform cam;

    protected void Awake()
    {
        characterPhysics = GetComponent<Character_Physics>();
        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();
        wallInteraction = GetComponent<Wall_Interaction>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        inputDirection = GetMoveDirection();
        pressMovementCommand = CreatePressCommand(inputKeys);

        if (characterPhysics.currentGravityState == GravityState.OnWall)
        {
            characterPhysics.inputDirection = wallInteraction.CalculateDirectionOnWall(inputDirection);
            if (pressMovementCommand.Equals(jumpCommand))
                wallInteraction.JumpOffWall(); 
        }
        else
        {
            characterPhysics.inputDirection = CalculateDirectionOnGround();
            doublePressMovementCommand = CreateDoublePressCommand(inputKeys);

            if (doublePressMovementCommand.Equals(dashBackwardCommand))
                dash.CastDash();

            if (pressMovementCommand.Equals(jumpCommand))
                jump.CastJump();
        }
    }

    private Vector3 CalculateDirectionOnGround()
    {  
        if (inputDirection.magnitude >= 0.2f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            return movementDirection;
        }
        return Vector3.zero;
    }

    

    private Vector3 GetMoveDirection()
    {
        float ad = Input.GetAxisRaw("Horizontal");
        float ws = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(ad, 0f, ws).normalized;

        return direction;
    }

    protected bool GetHelperKeyPressed(KeyCode helperKey)
    {
        if (Input.GetKey(helperKey))
            return true;
        else
            return false;
    }

    protected virtual InputCommand CreatePressCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyDown(inputKey))
                return new InputCommand(inputKey, false, false);
        }
        return new InputCommand();
    }

    protected virtual InputCommand CreateReleaseCommand(KeyCode[] inputKeys)
    {
        foreach(KeyCode inputKey in inputKeys)
        {
            if (Input.GetKeyUp(inputKey))
                return new InputCommand(inputKey, true, false);
        }
        return new InputCommand();
    }

    protected virtual InputCommand CreateDoublePressCommand(KeyCode[] inputKeys)
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
}
