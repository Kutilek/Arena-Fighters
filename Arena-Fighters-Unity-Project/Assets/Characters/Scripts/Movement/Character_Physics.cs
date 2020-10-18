using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character_Physics : MonoBehaviour
{
    private readonly float gravity = Physics.gravity.y;
    private const float gravityMultiplier = 1.8246f;
    private const float gravityOnGround = -2f;
    protected CharacterController controller;

    // Ground Checking
    protected Transform groundCheck;
    protected LayerMask groundMask;
    protected float groundDistance = 0.1f;

    // Movement
    protected float currentSpeed;
    protected float currentFallSpeed;
    protected Vector3 velocity;
    protected Vector3 currentImpact;
    protected Vector3 currentOutsideImpact;
    public bool checkForGround = true;
    public Vector3 inputDirection;
    
    // States
    public GravityState currentGravityState;
    protected MovementImpairingEffect currentMovementImpairingEffect = MovementImpairingEffect.None;

    // Physics values
    [SerializeField] protected float damping;
    [SerializeField] protected float mass;

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
        groundMask = LayerMask.GetMask("Ground");
        groundCheck = transform.Find("GroundCheck");
    }

    protected void Update()
    {
        currentSpeed = 4f;

        CheckIfGrounded();
        RotateOnGround();

        if (currentGravityState == GravityState.Falling || currentGravityState == GravityState.JumpedOffWall)
            AddGravity();

        if (currentMovementImpairingEffect != MovementImpairingEffect.Stun)
            MoveCharacter(inputDirection);
    }

    public IEnumerator PauseGroundCheck()
    {
        checkForGround = false;

        yield return new WaitForSeconds(0.7f);

        checkForGround = true;
    }

    protected void CheckIfGrounded()
    {
        if (checkForGround)
        {
            if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
            {
                currentGravityState = GravityState.Grounded;
                currentFallSpeed = gravityOnGround;
            }
        }   
    }

    private float turnSmoothVelocity;
    private void RotateOnGround()
    {
        if (currentGravityState == GravityState.Grounded)
        {
            float turnSmoothTime = 0.05f;
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (inputDirection.magnitude >= 0.1f)
                transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
        }          
    }

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

    public IEnumerator SetMovementImpairingEffect(MovementImpairingEffect effect, float length)
    {
        currentMovementImpairingEffect = effect;

        yield return new WaitForSeconds(length);

        currentMovementImpairingEffect = MovementImpairingEffect.None;
    }
}
