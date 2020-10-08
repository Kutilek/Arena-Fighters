using System.Collections;
using UnityEngine;
using Cinemachine;

public class Player_Controller : Physics_Character_Controller
{
    #region Movement Inputs

    // Movement Commands
    private Command dashForwardCommand = new Command(KeyCode.W, false, true);
    private Command dashLeftCommand = new Command(KeyCode.A, false, true);
    private Command dashBackwardCommand = new Command(KeyCode.S, false, true);
    private Command dashRightCommand = new Command(KeyCode.D, false, true);
    private Command jumpCommand = new Command(KeyCode.Space, false, false);

    // Current Movement Inputs
    public Command pressMovementCommand;
    public Command doublePressMovementCommand;
    public Vector3 inputDirection;
    public bool moveHelperKeyPressed;

    #endregion

    // Transforms needed for player movement
    private Transform cam;
    private Transform closestEnemy;

    #region Movement Values

    // Speeds
    [SerializeField] protected float frontSpeed;
    [SerializeField] protected float sideSpeed;
    [SerializeField] protected float backSpeed;
    [SerializeField] protected float wallSpeed;

    // Forces
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float frontDashForce;
    [SerializeField] protected float sideDashForce;
    [SerializeField] protected float backDashForce;

    // Durations
    [SerializeField] protected float dashDuration;
    [SerializeField] protected float onWallDuration;

    private float dashBonus = 1f;
    private bool dashed = false;
    private bool jumpedOfWall = false;

    #endregion

    #region Attack Inputs

    // Attack Commands
    private Command lightAttackCommand = new Command(KeyCode.Mouse0, false, false);
    private Command heavyAttackCommand = new Command(KeyCode.Mouse1, false, false);
    private Command specialAttackCommand = new Command(KeyCode.Mouse2, false, false);
    private Command actionOneStartCommand = new Command(KeyCode.Q, false, false);
    private Command actionOneEndCommand = new Command(KeyCode.Q, true, false);
    private Command actionTwoStartCommand = new Command(KeyCode.E, false, false);
    private Command actionThreeStartCommand = new Command(KeyCode.F, false, false);

    // Current Attack Inputs
    public Command pressAttackCommand;
    public Command releaseAttackCommand;
    public bool attackHelperKeyPressed;
    public float mouseScroll;

    #endregion

    protected override void Start()
    {
        base.Start();       
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall" && !isGrounded && velocity.y > -5f && falling)
            StartCoroutine(OnWallEnter());
    }

    protected void Update()
    {
        Vector3 moveDir = new Vector3();

        LoadClosestEnemy();
        CheckIfGrounded();

        if (!onWall)
        {
            SetSpeed();

            if (inputDirection.magnitude > 0.1f)
                moveDir = CalculateDirectionOnGround();

            CheckForDashInput();
            RotateOnGround();
        }
        else
        {
            currentSpeed = wallSpeed;

            if (Mathf.Abs(inputDirection.x) > 0.5f)
                moveDir = CalculateDirectionOnWall();

            if (pressMovementCommand.Equals(jumpCommand))
                JumpOffWall();

            RotateOnWall();
        }

        if (!falling)
        {
            if (pressMovementCommand.Equals(jumpCommand))
                StartCoroutine(JumpOffGround());
        }
        else
            AddGravity();

        if (jumpedOfWall && pressMovementCommand.Equals(jumpCommand))
            AddForce(Vector3.down, jumpForce);

        MoveCharacter(moveDir);
    }

    protected void LoadClosestEnemy()
    {  
        bool isEnemyClose = Physics.CheckSphere(transform.position, 5f, LayerMask.GetMask("Enemy"));

        if (isEnemyClose)
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Enemy"));
            foreach (Collider col in collider)
                closestEnemy = col.transform;
        }
        else
            closestEnemy = null;
    }

    #region Ground Movement

    private float turnSmoothVelocity;
    private void RotateOnGround()
    {
        float turnSmoothTime = 0.05f;
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (inputDirection.magnitude >= 0.1f && !falling)
        {
            if (inputDirection.z > 0.5f)
                transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
        }
    }

    private Vector3 CalculateDirectionOnGround()
    {  
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        return movementDirection;
    }

    private IEnumerator ResetDash()
    {
        dashed = true;

        yield return new WaitForSeconds(3f);

        dashed = false;
    }

    private IEnumerator Dash(float dashForce)
    {
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle, 0f) * Vector3.forward;

        StartCoroutine(ResetDash());
        if (falling)
            currentFallSpeed -= 15f;
        AddForce(dashDirection, dashForce);

        yield return new WaitForSeconds(dashDuration);

        if (!falling)
            ResetImpact();
    }

    private void CheckForDashInput()
    {
        if (!dashed && !jumpedOfWall)
        {
            if (doublePressMovementCommand.Equals(dashForwardCommand))
                StartCoroutine(Dash(frontDashForce * dashBonus));
            else if (doublePressMovementCommand.Equals(dashBackwardCommand))
                StartCoroutine(Dash(backDashForce * dashBonus));
            else if (doublePressMovementCommand.Equals(dashLeftCommand))
                StartCoroutine(Dash(sideDashForce * dashBonus));
            else if (doublePressMovementCommand.Equals(dashRightCommand))
                StartCoroutine(Dash(sideDashForce * dashBonus));
        }
    }

    private IEnumerator JumpOffGround()
    {
        AddForce(Vector3.up, jumpForce);
        falling = true;

        yield return new WaitUntil(() => isGrounded && !falling);

        ResetImpactY();
    }

    // Method for setting speed and dash bonus
    private void SetSpeed()
    {
        if (inputDirection.z > 0.5f)
            currentSpeed = frontSpeed;
        else if (Mathf.Abs(inputDirection.x) > 0.5f)
            currentSpeed = sideSpeed;
        else
            currentSpeed = backSpeed;
    
        if (moveHelperKeyPressed)
        {
            currentSpeed = CalculateSpeedBonus(currentSpeed);
            dashBonus = CalculateDashBonus(dashBonus);
        }
    }

    // Calculator functions for speed bonus and dash bonus
    private float CalculateSpeedBonus(float speed)
    {
        speed = speed + ((3f * speed) / 4f);
        if (speed > 3f)
            speed -= 1f;
        return speed;
    }

    private float CalculateDashBonus(float dashBonus)
    {
        dashBonus = 1.65f;
        return dashBonus;
    }  

    #endregion

    #region Wall Movement

    private IEnumerator OnWallEnter()
    {
        onWall = true;
        falling = false;
        currentFallSpeed = 0f;

        yield return new WaitForSeconds(onWallDuration);

        onWall = false;
        falling = true;
        currentFallSpeed -= 5f;
    }

    private void RotateOnWall()
    {
        transform.LookAt(arenaCenter);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }

    private Vector3 CalculateDirectionOnWall()
    {
        float targetAngle = Mathf.Atan2(inputDirection.x, 0f) * Mathf.Rad2Deg;
        Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward;
        return movementDirection;
    }

    private IEnumerator ResetJumpOfWall()
    {
        jumpedOfWall = true;

        yield return new WaitUntil(() => isGrounded);

        jumpedOfWall = false;
    }

    private void JumpOffWall()
    {
        AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * new Vector3(0f, 0.5f, 1f), jumpForce * 2f);
        StopCoroutine(OnWallEnter());
        onWall = false;
        StartCoroutine(ResetJumpOfWall());
    }

    #endregion
}
