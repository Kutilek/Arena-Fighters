using UnityEngine;

public abstract class Character_Behaviour : MonoBehaviour, IPlayerMovement
{
    private CharacterController controller;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    protected Transform cam;

    public float walkSpeed;
    public float runSpeed;
    
    public virtual void Walk(Vector3 direction)
    {
        Debug.Log("I am walking");
    }

    public virtual void Run(Vector3 direction)
    {
        Debug.Log("I am running");
    }

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    protected virtual void Move(Vector3 direction, float speed)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }  
}
