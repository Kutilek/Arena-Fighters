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
    private float lastDashTime;
    private bool runToWall;
    [SerializeField] protected float playerDashDistance;
    [SerializeField] protected float dashCooldown;

    protected override void Start()
    {
        base.Start();
    }

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

        yield return new WaitForSecondsRealtime(followPlayerCommand.duration);

        direction = Vector3.zero;
        followPlayerCommand.isAble = false;
    }

    protected void Update()
    {
        CheckIfGrounded();
        directionToPlayer = -GetDirectionToObject(player);
        directionToWall = GetDirectionToObject(arenaCenter);
        distanceToPlayer = GetDistanceToObject(player);
        distanceToWall = GetDistanceToObject(arenaCenter);

        if (!moveRandomCommand.isAble)
            moveRandomCommand.CalculateChance();     
        else 
            StartCoroutine(MoveRandom());

        followPlayerCommand.CalculateChance();

        if (followPlayerCommand.isAble)
            StartCoroutine(FollowPlayer());

            
        
/*
        if (distanceToPlayer <= playerDashDistance && Time.time - lastDashTime > dashCooldown)
            StartCoroutine(DashToPlayer());
            
        SetDirection();

        if (Input.GetKey(KeyCode.V))
            moveRandomly = true;
        else
        {
            moveRandomly = false;
            RandomX = 0f;
            RandomZ = 0f;
        }*/
            
        if (!falling)
        {
            if (runToWall && distanceToWall > 29.5f && isGrounded)
                StartCoroutine(JumpOffGround());
        }
        else
            AddGravity();

        currentSpeed = 5f;

        MoveCharacter(direction);
        
        RotateOnGround();
    }

    protected IEnumerator DashToPlayer()
    {
        AddForce(directionToPlayer, 20f);
        
        yield return new WaitForSeconds(0.2f);
 
        lastDashTime = Time.time;
        ResetImpact();
    }

    private IEnumerator OnWallEnter()
    {
        onWall = true;
        falling = false;
        currentFallSpeed = 0f;

        yield return new WaitForSeconds(5f);

        onWall = false;
        falling = true;
        currentFallSpeed -= 5f;
        RandomX = 0f;
    }

    

  /*  protected void SetDirection()
    {
        if (!onWall)
        {
            if (followPlayer)
                direction = directionToPlayer;
            else if (runToWall)
                direction = directionToWall;
            else if (moveRandomly)
            {
                if (distanceToWall > 25f)
                    direction = -directionToWall;
                else if(RandomX == 0f || RandomZ == 0f)
                {
                    RandomX = Random.Range(-1f, 1f);
                    RandomZ = Random.Range(-1f, 1f);

                    direction = new Vector3(RandomX, 0f, RandomZ);    
                }      
            }
            else
                direction = new Vector3();
        }  
        else
        {
            transform.LookAt(arenaCenter);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            direction = CalculateDirectionOnWall();
            chance = Random.Range(0f, 100f);
            if (chance > 99f)
                JumpOffWall();
        }
    }*/

    private void JumpOffWall()
    {
        AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * new Vector3(0f, 0.5f, 1f), 25f * 2f);
        StopCoroutine(OnWallEnter());
        onWall = false;
        falling = true;
        RandomX = 0f;
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
