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
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.05f;
    
    private Vector3 currentImpact;
    private float currentSpeed;
    private float dashBonus;

    [SerializeField] protected float frontSpeed;
    [SerializeField] protected float sideSpeed;
    [SerializeField] protected float backSpeed;
    [SerializeField] protected float frontDashForce;
    [SerializeField] protected float sideDashForce;
    [SerializeField] protected float backDashForce;
    [SerializeField] protected float damping;
    [SerializeField] protected float mass;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        SetSpeed();

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
               
        Walk(currentSpeed);   
    }

    void SetSpeed()
    {
        dashBonus = 1f;

        if (direction.z > 0.5f)
            currentSpeed = frontSpeed;
        else
            currentSpeed = backSpeed;

        if (Mathf.Abs(direction.x) > 0.5f)
            currentSpeed = sideSpeed;

        if (moveHelperKeyPressed)
        {
            currentSpeed *= 2f;
            dashBonus = 2f;
        }
    }

    public virtual IEnumerator Dash(float dashForce, float direction)
    {
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + direction, 0f) * Vector3.forward;

        currentImpact += dashDirection.normalized * dashForce / mass;

        yield return new WaitForSeconds(0.2f);
    }

    void RotatePlayer()
    {
        float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, CalculateDirectionAngle(), ref turnSmoothVelocity, turnSmoothTime);

        if (direction.z > 0.5f)
            transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);  
    }

    float CalculateDirectionAngle()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        return targetAngle;
    }

    protected void Walk(float speed)
    {
        Vector3 moveDir;
        Vector3 velocity = new Vector3();

        if (direction.magnitude >= 0.1f)
        {
            float direction = CalculateDirectionAngle();
            moveDir = Quaternion.Euler(0f, direction, 0f) * Vector3.forward;
            velocity = moveDir * speed;
            RotatePlayer();
        }

        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        controller.Move(velocity * Time.deltaTime);

        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
    }   
}
