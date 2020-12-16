using UnityEngine;

public class Physics_Player : Physics_Character
{
    public Vector3 inputDirectionRaw;
    private Transform cam;
    
    protected override void Awake()
    {
        base.Awake();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    protected override void Update()
    {
        base.Update();
        inputDirection = CalculateDirectionOnGround();
        animator.SetFloat("fallingSpeed", velocity.y);
    }

    protected override void RotateOnGround()
    {
        // Rotate only if Grounded
        if (currentGravityState == GravityState.Grounded)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

            if (inputDirectionRaw.magnitude >= 0.1f)
            {
                // Rotates only when moving forward, else player is rotated in the camera direction
                if (inputDirectionRaw.z > 0.5f)
                {
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    animator.SetBool("isWalking", true);
                }     
                else
                {
                    transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);   
                }
                    
            }
            else
                animator.SetBool("isWalking", false);
        }
    }

    private Vector3 CalculateDirectionOnGround()
    {  
        if (inputDirectionRaw.magnitude >= 0.2f)
        {
            float targetAngle = Mathf.Atan2(inputDirectionRaw.x, inputDirectionRaw.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            return movementDirection;
        }
        return Vector3.zero;
    }
}
