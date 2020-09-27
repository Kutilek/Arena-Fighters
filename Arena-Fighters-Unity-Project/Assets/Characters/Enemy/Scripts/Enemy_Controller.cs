using System.Collections;
using UnityEngine;

public class Enemy_Controller : Physics_Character_Controller
{
    private Transform player;
    private Vector3 direction;
    private Vector3 directionToPlayer;
    private Vector3 directionToWall;
    private float distanceToPlayer;
    private float distanceToWall;

    [SerializeField] protected float playerDashDistance;

    protected void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall" && !isGrounded && velocity.y > -5f && falling)
            StartCoroutine(OnWallEnter());
    }

    private float RandomX = 0f;
    private float RandomZ = 0f;
    [SerializeField] private EnemyCommand moveRandomCommand;

    private Vector3 SetRandomDirection()
    {
        Vector3 dir = Vector3.zero;

        if (distanceToWall >= 25f)
            dir = -directionToWall;
        else if(RandomX == 0f || RandomZ == 0f)
        {
            RandomX = Random.Range(-1f, 1f);
            RandomZ = Random.Range(-1f, 1f);

            dir = new Vector3(RandomX, 0f, RandomZ);
        }

        return dir;
    }

    private IEnumerator MoveRandom()
    {
        if (direction == Vector3.zero)
            direction = SetRandomDirection(); 

        if (distanceToPlayer <= 15f)
            transform.LookAt(player);
        else
            RotateOnGround();

        yield return new WaitForSecondsRealtime(moveRandomCommand.duration);

        direction = Vector3.zero;
        RandomX = 0f;
        RandomZ = 0f;
        moveRandomCommand.isAble = false;
    }

    [SerializeField] private EnemyCommand followPlayerCommand;

    private IEnumerator FollowPlayer()
    {
        StopCoroutine(MoveRandom());
        direction = directionToPlayer;
        
        RotateOnGround();

        yield return new WaitForSecondsRealtime(followPlayerCommand.duration);

        direction = Vector3.zero;
        followPlayerCommand.isAble = false;
    }

    [SerializeField] private EnemyCommand dashToPlayerCommand;

    protected IEnumerator DashToPlayer()
    {
        AddForce(directionToPlayer, 20f);
        
        yield return new WaitForSeconds(0.2f);

        dashToPlayerCommand.isAble = false;
        ResetImpact();
    }

    [SerializeField] private EnemyCommand runToWallCommand;

    private bool calculateChances = true;

    protected void Update()
    {
        CheckIfGrounded();
        directionToPlayer = -GetDirectionToObject(player);
        directionToWall = GetDirectionToObject(arenaCenter);
        distanceToPlayer = GetDistanceToObject(player);
        distanceToWall = GetDistanceToObject(arenaCenter);

        if (calculateChances)
        {
            moveRandomCommand.CalculateChance();
            followPlayerCommand.CalculateChance();
            dashToPlayerCommand.CalculateChance();
            runToWallCommand.CalculateChance();
        }
        
        dashToPlayerCommand.ResetIsAble();

        if (moveRandomCommand.isAble)
            StartCoroutine(MoveRandom());

        if (followPlayerCommand.isAble)
        {
            StartCoroutine(FollowPlayer());
            if (distanceToPlayer <= playerDashDistance && dashToPlayerCommand.isAble)
                StartCoroutine(DashToPlayer());
        }

        if (runToWallCommand.isAble)
        {
            calculateChances = false;
            StopAllCoroutines();
            moveRandomCommand.isAble = false;
            followPlayerCommand.isAble = false;
            dashToPlayerCommand.isAble = false;
            RotateOnGround();

            direction = directionToWall; 
            if (distanceToWall > 29.5f && isGrounded)
                StartCoroutine(JumpOffGround());
        }

        if (onWall)
        {
            jumpOffWallCommand.CalculateChance();
            if (jumpOffWallCommand.isAble)
                JumpOffWall();
        }

        if (falling)
            AddGravity();

        currentSpeed = 4f;

        MoveCharacter(direction);
    }

    [SerializeField] private EnemyCommand jumpOffWallCommand;

    private IEnumerator OnWallEnter()
    {
        onWall = true;
        falling = false;
        currentFallSpeed = 0f;
        direction = Vector3.zero;
        runToWallCommand.isAble = false;
        
        transform.LookAt(arenaCenter);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        direction = CalculateDirectionOnWall();

        yield return new WaitForSeconds(5f);

        onWall = false;
        calculateChances = true;
        falling = true;
        currentFallSpeed -= 5f;
        RandomX = 0f;
        direction = Vector3.zero;
    }

    private void JumpOffWall()
    {
        currentFallSpeed -= 5f;
        AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * new Vector3(0f, 0.5f, 1f), 50f * 2f);
        StopCoroutine(OnWallEnter());
        onWall = false;
        calculateChances = true;
        falling = true;  
        RandomX = 0f;
        direction = Vector3.zero;
        jumpOffWallCommand.isAble = false;
    }

    private Vector3 CalculateDirectionOnWall()
    {
        if (RandomX == 0f)
            RandomX = Random.Range(-1f, 1f);

        float targetAngle = Mathf.Atan2(RandomX, 0f) * Mathf.Rad2Deg;
        Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward;
        return movementDirection;
    }

    protected Vector3 GetDirectionToObject(Transform obj)
    {
        Vector3 direction = (transform.position - obj.transform.position).normalized;
        return direction;
    }

    protected float GetDistanceToObject(Transform obj)
    {
        float distance = Vector3.Distance(transform.position, obj.transform.position);
        return distance;
    }

    private IEnumerator JumpOffGround()
    {
        AddForce(Vector3.up, 50f);
        falling = true;

        yield return new WaitUntil(() => isGrounded && !falling);

        ResetImpactY();
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
