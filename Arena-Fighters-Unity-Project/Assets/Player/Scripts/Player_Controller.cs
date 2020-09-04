using System.Collections;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    #region Movement Commands

    // Forward Movement Commands
    private Command dashForward = new Command(KeyCode.W, false, true);
    private Command dashForwardSpecial = new Command(KeyCode.W, false, true);

    // Left Movement Commands
    private Command dashLeft = new Command(KeyCode.A, false, true);
    private Command dashLeftSpecial = new Command(KeyCode.A, false, true);

    // Backward Movement Commands
    private Command dashBackward = new Command(KeyCode.S, false, true);
    private Command dashBackwardSpecial = new Command(KeyCode.S, false, true);

    // Right Movement Commands
    private Command dashRight = new Command(KeyCode.D, false, true);
    private Command dashRightSpecial = new Command(KeyCode.D, false, true);

    // Jump Movement Commands
    private Command jump = new Command(KeyCode.Space, false, false);

    public Command movementCommand;

    #endregion
 
    private CharacterController controller;
    public bool moveHelperKeyPressed;
    public Vector3 direction; 

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Transform cam;
    private Vector3 currentImpact;
    
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float damping;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void FixedUpdate()
    {
        float speed = walkSpeed;
        if (moveHelperKeyPressed)
            speed = runSpeed;

        Walk(speed);      
    }

    protected void Walk(float speed)
    {
        Vector3 moveDir;
        Vector3 velocity = new Vector3();

        if (direction.magnitude >= 0.1f)
        {
            moveDir = CalculateMoveDir(direction);
            velocity = moveDir * speed;
        }

        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        controller.Move(velocity * Time.deltaTime);

        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
    }

    protected Vector3 CalculateMoveDir(Vector3 inputDirection)
    {
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }   
}
