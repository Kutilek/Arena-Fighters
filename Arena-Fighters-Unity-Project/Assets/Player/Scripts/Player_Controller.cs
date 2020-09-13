using System.Collections;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    #region Movement Commands

    private Command dashForward = new Command(KeyCode.W, false, true);
    private Command dashLeft = new Command(KeyCode.A, false, true);
    private Command dashBackward = new Command(KeyCode.S, false, true);
    private Command dashRight = new Command(KeyCode.D, false, true);
    private Command jump = new Command(KeyCode.Space, false, false);

    public Command movementCommand;

    #endregion

    public bool moveHelperKeyPressed;
    public Vector3 direction;

    private CharacterController controller;
    private Transform cam;
    private Transform arenaCenter;
    public Transform groundCheck;
    private LayerMask groundMask;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.05f;

    private readonly float gravity = Physics.gravity.y;
    private float groundDistance = 0.1f;
    private bool isGrounded;
    private float currentSpeed;
    private float dashBonus;
    public bool wallRun;
    public Vector3 velocity;
    
    private Vector3 currentImpact;
    public float velocityY;

    [SerializeField] protected float frontSpeed;
    [SerializeField] protected float sideSpeed;
    [SerializeField] protected float backSpeed;
    [SerializeField] protected float frontDashForce;
    [SerializeField] protected float sideDashForce;
    [SerializeField] protected float backDashForce;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float damping;
    [SerializeField] protected float mass;
    
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

        if (!wallRun)
        {
            HandleGravity();

            if (movementCommand.Equals(dashForward))
            {
                StartCoroutine(Dash(frontDashForce * dashBonus, 0f));
            }
            else if (movementCommand.Equals(dashBackward))
            {
                StartCoroutine(Dash(backDashForce * dashBonus, 180f));
            }
            else if (movementCommand.Equals(dashLeft))
            {
                StartCoroutine(Dash(sideDashForce * dashBonus, 270f));
            }   
            else if (movementCommand.Equals(dashRight))
            {
                StartCoroutine(Dash(sideDashForce * dashBonus, 90f));
            }

            moveDir = cam.transform.TransformDirection(direction);
        }
        else
        {
            if (Mathf.Abs(direction.x) > 0.5f)
            {
                float targetAngle = Mathf.Atan2(direction.x, 0f) * Mathf.Rad2Deg;
                moveDir = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward * 5f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * Vector3.forward, 100f);
                StopCoroutine(WallRun());
                wallRun = false;
            }

        }

        velocity = moveDir * currentSpeed + Vector3.up * velocityY;
        
        CheckIfGrounded();
        SetSpeed();

        RotatePlayer(); 

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        HandleCurrentImpact();

        controller.Move(velocity * Time.deltaTime);  
    }

    private IEnumerator WallRun()
    {
        wallRun = true;
        velocityY = 0f;

        yield return new WaitForSeconds(80f);

        velocityY -= 5f;
        wallRun = false;

    }

    public virtual void HandleCurrentImpact()
    {
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
    } 

    public void Jump()
    {
        if (isGrounded)
        {
            AddForce(Vector3.up, jumpForce);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall" && !isGrounded && velocity.y > -5f)
        {
            StartCoroutine(WallRun());
        }
   }

    public IEnumerator Dash(float dashForce, float plusDir)
    {
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + plusDir, 0f) * Vector3.forward;
        AddForce(dashDirection, dashForce);

        yield return new WaitForSeconds(0.2f);

        ResetImpact();
    }

    public virtual void HandleGravity()
    {
        if (isGrounded && velocityY < 0f)
        {
            velocityY = 0f;
        }

        velocityY += gravity * 1.5f * Time.deltaTime;
    }

    public void AddForce(Vector3 dir, float magnitude)
    {
        currentImpact += dir.normalized * magnitude / mass;
    }

    public void ResetImpact()
    {
        currentImpact = Vector3.zero;
    }

    public void ResetImpactY()
    {
        currentImpact.y = 0f;
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void SetSpeed()
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
            currentSpeed *= 1.678423f;
            dashBonus = 2f;
        }
    }

    void RotatePlayer()
    {

        
        if (!wallRun)
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
        else
        {
            transform.LookAt(arenaCenter);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }
              
    }
}
