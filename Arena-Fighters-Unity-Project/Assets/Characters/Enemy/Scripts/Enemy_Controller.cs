using System.Collections;
using UnityEngine;

public class Enemy_Controller : Physics_Character_Controller
{
    private Transform player;
    private Vector3 direction;
    private Vector3 directionToPlayer;
    private float distanceToPlayer;
    private bool followPlayer;
    [SerializeField] protected float playerDashDistance;

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
        directionToPlayer = GetDirectionToPlayer();
        distanceToPlayer = GetDistanceToPlayer();

        if (distanceToPlayer <= playerDashDistance)
            StartCoroutine(DashToPlayer());
            
        SetDirection();

        currentSpeed = 5f;

        MoveCharacter(direction);
        
        RotateOnGround();
    }

    protected IEnumerator DashToPlayer()
    {
        AddForce(directionToPlayer, 20f);

        yield return new WaitForSeconds(0.1f);

        ResetImpact();
    }

    protected void SetDirection()
    {
        if (followPlayer)
            direction = directionToPlayer;
        else
            direction = new Vector3();
    }

    protected Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = (transform.position - player.transform.position).normalized;
        return -direction;
    }

    protected float GetDistanceToPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance;
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
