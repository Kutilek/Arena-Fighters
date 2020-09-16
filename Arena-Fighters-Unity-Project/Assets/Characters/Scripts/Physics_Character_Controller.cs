using UnityEngine;

public abstract class Physics_Character_Controller : MonoBehaviour
{
    private readonly float gravity = Physics.gravity.y;
    private const float gravityMultiplier = 1.8246f;
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
    
    // States
    protected bool isGrounded;
    protected bool onWall;
    protected bool falling;

    // Physics values
    [SerializeField] protected float damping;
    [SerializeField] protected float mass;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();        
        groundMask = LayerMask.GetMask("Ground");
        groundCheck = transform.Find("GroundCheck");
    }

    protected void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            currentFallSpeed = -0.1f;
            falling = false;
        }
    }

    protected void MoveCharacter(Vector3 direction)
    {
        velocity = direction.normalized * currentSpeed + Vector3.up * currentFallSpeed;

        if (currentImpact.magnitude > 0.2f)
            velocity += currentImpact;

        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }

    protected void AddGravity()
    {
        currentFallSpeed += gravity * gravityMultiplier * Time.deltaTime;
    }

    protected void AddForce(Vector3 direction, float magnitude)
    {
        currentImpact += direction.normalized * magnitude / mass;
    }

    protected void ResetImpact()
    {
        currentImpact = Vector3.zero;
    }
}
