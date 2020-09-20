using System.Collections;
using UnityEngine;

public class Enemy_Controller : Physics_Character_Controller
{
    private Transform player;
    private Vector3 direction;
    private Vector3 directionToPlayer;
    private float distanceToPlayer;

    protected override void Start()
    {
        base.Start();    
    }

    protected void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void Update()
    {
        CheckIfGrounded();
        directionToPlayer = (transform.position - player.transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        currentSpeed = 5f;

        MoveCharacter(direction);
        
        RotateOnGround();
    }

    private void Jump()
    {
        AddForce(Vector3.up, 50f);
        falling = true;
    }

    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.05f;
    private void RotateOnGround()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (direction.magnitude >= 0.1f && !falling)
        {
            transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
        }    
    }
}
