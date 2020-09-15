using System.Collections;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    #region Movement Inputs

    // Commands
    private Command dashForward = new Command(KeyCode.W, false, true);
    private Command dashLeft = new Command(KeyCode.A, false, true);
    private Command dashBackward = new Command(KeyCode.S, false, true);
    private Command dashRight = new Command(KeyCode.D, false, true);
    private Command jump = new Command(KeyCode.Space, false, false);

    // Current Inputs
    public Command pressMovementCommand;
    public Command doublePressMovementCommand;
    public Vector3 direction;
    public bool moveHelperKeyPressed;

    #endregion

    // Physics & Movement
    private readonly float gravity = Physics.gravity.y;
    public Transform groundCheck;
    private LayerMask groundMask;
    private float velocityY;
    private float groundDistance = 0.1f;
    private bool isGrounded;
    private Vector3 velocity;
    private Vector3 currentImpact;
    [SerializeField] protected float damping;
    [SerializeField] protected float mass;
    private float currentSpeed;
    private float dashBonus;
    private bool wallRunning;
    private bool falling;

    private CharacterController controller;
    private Transform cam;
    private Transform arenaCenter;
    
    [SerializeField] protected float frontSpeed;
    [SerializeField] protected float sideSpeed;
    [SerializeField] protected float backSpeed;
    [SerializeField] protected float frontDashForce;
    [SerializeField] protected float sideDashForce;
    [SerializeField] protected float backDashForce;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float wallRunDuration;
    [SerializeField] protected float dashDuration;

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        arenaCenter = GameObject.FindGameObjectWithTag("ArenaCenter").transform;
        groundMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        Vector3 moveDir = new Vector3();

        SetSpeed();
        CheckIfGrounded();

        if (!wallRunning)
        {
            if (doublePressMovementCommand.Equals(dashForward))
                StartCoroutine(Dash(frontDashForce * dashBonus));
            else if (doublePressMovementCommand.Equals(dashBackward))
                StartCoroutine(Dash(backDashForce * dashBonus));
            else if (doublePressMovementCommand.Equals(dashLeft))
                StartCoroutine(Dash(sideDashForce * dashBonus));
            else if (doublePressMovementCommand.Equals(dashRight))
                StartCoroutine(Dash(sideDashForce * dashBonus));

            if (direction.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            RotatePlayer();
        }
        else
        {
            transform.LookAt(arenaCenter);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            
            if (Mathf.Abs(direction.x) > 0.5f)
            {
                float targetAngle = Mathf.Atan2(direction.x, 0f) * Mathf.Rad2Deg;
                moveDir = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward * 5f;
            }   

            if (pressMovementCommand.Equals(jump))
            {
                AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * Vector3.forward, 100f);
                StopCoroutine(WallRun());
                wallRunning = false;
            }
        }      

        velocity = moveDir * currentSpeed + Vector3.up * velocityY;

        if (!falling)
        {    
            if (pressMovementCommand.Equals(jump))
                Jump();         
        }
        else
            AddGravity();

        CalculateCurrentImpact();
        controller.Move(velocity * Time.deltaTime);
    }

    private IEnumerator WallRun()
    {
        wallRunning = true;
        falling = false;
        velocityY = 0f;

        yield return new WaitForSeconds(wallRunDuration);

        velocityY -= 5f;
        wallRunning = false;
        falling = true;
    }

    private void CalculateCurrentImpact()
    {
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
    } 

    private void Jump()
    {
        falling = true;
        AddForce(Vector3.up, jumpForce);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall" && !isGrounded && velocity.y > -5f)
        {
            StartCoroutine(WallRun());
        }
    }

    private IEnumerator Dash(float dashForce)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle, 0f) * Vector3.forward;
        AddForce(dashDirection, dashForce);

        yield return new WaitForSeconds(dashDuration);

        ResetImpact();
    }

    private void AddGravity()
    {
        velocityY += gravity * 1.5f * Time.deltaTime;
    }

    private void AddForce(Vector3 dir, float magnitude)
    {
        currentImpact += dir.normalized * magnitude / mass;
    }

    private void ResetImpact()
    {
        currentImpact = Vector3.zero;
    }

    private void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            velocityY = 0f;
            falling = false;
        }      
    }

    private void SetSpeed()
    {
        dashBonus = 1f;

        if (direction.z > 0.5f)
            currentSpeed = frontSpeed;
        else if (Mathf.Abs(direction.x) > 0.5f)
            currentSpeed = sideSpeed;
        else
            currentSpeed = backSpeed;

        if (moveHelperKeyPressed)
        {
            currentSpeed = CalculateSpeedBonus(currentSpeed);
            dashBonus = CalculateDashBonus(dashBonus);
        }
    }

    private float CalculateSpeedBonus(float speed)
    {
        speed *= 1.678423f;
        return speed;
    }

    private float CalculateDashBonus(float bonus)
    {
        bonus = 2f;
        return bonus;
    }


    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.05f;

    private void RotatePlayer()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (direction.magnitude >= 0.1f)
        {
            if (direction.z > 0.5f)
                transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);  
        }    
    }
}
