﻿using UnityEngine;

[RequireComponent(typeof(Physics_Player))]
public class Input_Player : MonoBehaviour
{
    // Components
    private Physics_Player characterPhysics;

    private Command jump;
    private Command swordOut;
    private Command swordSlash;

    private void Start()
    {
        if (doublePressTime == 0f)
            doublePressTime = 0.25f;
        characterPhysics = GetComponent<Physics_Player>();
        Cursor.lockState = CursorLockMode.Locked;
       // jump = new Command(KeyCode.Space, GetComponent<Jump>());
        swordOut = new Command(KeyCode.LeftShift, GetComponent<Sword_Out>());
        swordSlash = new Command(KeyCode.Mouse0, GetComponent<Sword_Slash>());
    }

    private void Update()
    {
        characterPhysics.inputDirectionRaw = GetMoveDirection();
     /*   if (Input.GetKeyDown(jump.keyCode))
            jump.ability.Cast();*/

        if (Input.GetKeyDown(swordOut.keyCode))
            swordOut.ability.Cast();

        if (Input.GetKeyDown(swordSlash.keyCode))
            swordSlash.ability.Cast();
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