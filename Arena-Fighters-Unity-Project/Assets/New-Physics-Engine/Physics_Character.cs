using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Speed))]
public abstract class Physics_Character : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController controller;

    // Gravity
    private readonly float gravity = Physics.gravity.y * gravityMultiplier;
    private const float gravityMultiplier = 1.8246f;
    private const float gravityOnGround = -2f;
    
    // Ground Checking
    protected Transform groundCheck;
    protected LayerMask groundMask;
    protected float groundDistance = 0.1f;
    protected bool checkForGround = true;

    // Movement Values
    protected Vector3 inputDirection;
    protected float currentSpeed;
    protected float currentFallSpeed;
    protected Vector3 velocity;
    protected Vector3 currentImpact;
    protected Vector3 currentOutsideImpact;
    
    // States
    public GravityState currentGravityState;
    protected MovementImpairingEffect currentMovementImpairingEffect = MovementImpairingEffect.None;

    // Physics Object Values
    [SerializeField] protected float damping;
    [SerializeField] protected float mass;

    #region Physics Modifiers

    public void ResetGravity()
    {
        currentFallSpeed = 0f;
    }

    public void AddForce(Vector3 direction, float magnitude)
    {
        currentImpact += direction.normalized * magnitude / mass;
    }

    public void AddOutsideForce(Vector3 direction, float magnitude)
    {
        currentOutsideImpact += direction.normalized * magnitude / mass;
    }
    public void ResetImpact()
    {
        currentImpact = Vector3.zero;
    }

    public void ResetOutsideImpact()
    {
        currentOutsideImpact = Vector3.zero;
    }

    public void ResetImpactY()
    {
        currentImpact.y = 0f;
        currentOutsideImpact.y = 0f;
    }

    // Sets Movement Impairing Effect
    public IEnumerator SetMovementImpairingEffect(MovementImpairingEffect effect, float length)
    {
        currentMovementImpairingEffect = effect;

        yield return new WaitForSeconds(length);

        currentMovementImpairingEffect = MovementImpairingEffect.None;
    }

    public IEnumerator PauseCheckForGround(float length)
    {
        checkForGround = false;

        yield return new WaitForSeconds(length);

        checkForGround = true;
    }

    #endregion

    protected virtual void Awake()
    {
        if (damping == 0f)
            damping = 6f;
        if (mass == 0f)
            mass = 3f;

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");
        groundCheck = transform.Find("GroundCheck");
    }

    protected virtual void Start()
    {
        currentSpeed = GetComponent<Speed>().GetAmount();
    }

    protected virtual void Update()
    {
        CheckIfGrounded();
        RotateOnGround();

        if (currentGravityState == GravityState.Grounded)
            currentFallSpeed = gravityOnGround;     
        else if (currentGravityState == GravityState.OnWall)
            currentFallSpeed = 0f;
        else
            AddGravity();
                   
        if (currentMovementImpairingEffect != MovementImpairingEffect.Stun)
            MoveCharacter(inputDirection);
    }

    protected float turnSmoothVelocity;
    protected virtual void RotateOnGround()
    {
        if (currentGravityState == GravityState.Grounded)
        {
            float turnSmoothTime = 0.05f;
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (inputDirection.magnitude >= 0.1f)
            {
                transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
                animator.SetFloat("walks", 1f);
            }
            else
                animator.SetFloat("walks", 0f);
        }
    }

    // Checks if check for ground is needed
    protected void CheckIfGrounded()
    {
        if (checkForGround)
        {
            if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
            {
                currentGravityState = GravityState.Grounded;
                animator.SetBool("grounded", true);
            }
            else
            {
                currentGravityState = GravityState.Falling;
                animator.SetBool("grounded", false);
            }
        }
    }

    // Sets the Character Velocity
    protected void MoveCharacter(Vector3 direction)
    {
        velocity = Vector3.up * currentFallSpeed;

        if (currentMovementImpairingEffect != MovementImpairingEffect.Immobilization)
        {
            velocity += direction.normalized * currentSpeed;

            CheckForImpact(currentImpact);
            currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
        }

        CheckForImpact(currentOutsideImpact);
        currentOutsideImpact = Vector3.Lerp(currentOutsideImpact, Vector3.zero, damping * Time.deltaTime);

        // Lerping must be done outside  CheckForImpact() or it does not work

        controller.Move(velocity * Time.deltaTime);
    }

    protected void CheckForImpact(Vector3 impact)
    {
        if (impact.magnitude > 1f)
            velocity += impact;
    }

    protected void AddGravity()
    {
        currentFallSpeed += gravity * (mass / 2f) * Time.deltaTime;
    }
}
