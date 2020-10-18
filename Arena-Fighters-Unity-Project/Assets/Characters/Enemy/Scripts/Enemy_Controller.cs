using System.Collections;
using UnityEngine;

public class Enemy_Controller : Character_Controller
{
    private Transform player;
    private Vector3 direction;
    private Vector3 directionToPlayer;
    private Vector3 directionToWall;
    private float distanceToPlayer;
    private float distanceToWall;
    private float RandomX = 0f;
    private float RandomZ = 0f;
    private bool calculateChances = true;

    #region Movement Values

    [SerializeField] private float playerDashDistance;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float onWallDuration;

    #endregion

    #region Commands

    [SerializeField] private EnemyCommand moveRandomCommand;
    [SerializeField] private EnemyCommand followPlayerCommand;
    [SerializeField] private EnemyCommand dashToPlayerCommand;
    [SerializeField] private EnemyCommand runToWallCommand;
    [SerializeField] private EnemyCommand jumpOffWallCommand;
    [SerializeField] private EnemyCommand stunPlayerCommand;

    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall" && !isGrounded && velocity.y > -5f && falling)
            StartCoroutine(OnWallEnter());
    }

    private Vector3 GetDirectionToObject(Transform obj)
    {
        Vector3 direction = (transform.position - obj.transform.position).normalized;
        return direction;
    }

    private float GetDistanceToObject(Transform obj)
    {
        float distance = Vector3.Distance(transform.position, obj.transform.position);
        return distance;
    }

    private void Attack()
    {
        if (distanceToPlayer <= 2f && followPlayerCommand.isAble)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.Log("HIT");
                  //  hit.collider.GetComponent<Player_Controller>().Knocback(transform.forward);
                }
            }
        }
    }

    protected IEnumerator MovementImpairEnemy(MovementImpairingEffects effect, float duration)
    {
        player.GetComponent<Player_Controller>().currentMovementImpairingEffect = effect;

        yield return new WaitForSeconds(duration);

        player.GetComponent<Player_Controller>().currentMovementImpairingEffect = MovementImpairingEffects.None;
    }


    private void StunPlayer()
    {
        if (followPlayerCommand.isAble && distanceToPlayer <= 10f && stunPlayerCommand.isAble)
        {
            Debug.Log("STUN");
            StartCoroutine(ResetStun());
            StartCoroutine(MovementImpairEnemy(MovementImpairingEffects.Immobilization, 7f));
        }
    }

    public bool stuned;
    private IEnumerator ResetStun()
    {
        stuned = true;

        yield return new WaitForSeconds(10f);

        stuned = false;
    }

    private void Update()
    {
        if (!stuned)
        {
            stunPlayerCommand.CalculateChance();
           // stunPlayerCommand.ResetIsAble();
            StunPlayer();
        }
        
        Attack();
        

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
            RotateInDirectionMoving();

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

        currentSpeed = speed;

        if (currentMovementImpairingEffect != MovementImpairingEffects.Stun)
            MoveCharacter(direction);
    }

    #region Ground Movement

    public void Knocback(Vector3 direction)
    {
        StartCoroutine(GetKnocback(direction));
    }

    public IEnumerator GetKnocback(Vector3 direction)
    {
        AddOutsideImpact(direction, 50f);

        yield return new WaitForSeconds(0.4f);

        ResetOutsideImpact();
    }

    private IEnumerator MoveRandom()
    {
        if (direction == Vector3.zero)
            direction = SetRandomDirection(); 

        if (distanceToPlayer <= 15f)
        {
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }
        else
            RotateInDirectionMoving();

        yield return new WaitForSecondsRealtime(moveRandomCommand.duration);

        RandomX = 0f;
        RandomZ = 0f;
        direction = Vector3.zero;
        moveRandomCommand.isAble = false;
    }

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

    private IEnumerator FollowPlayer()
    {
        StopCoroutine(MoveRandom());
        direction = directionToPlayer - Vector3.up;

        RotateInDirectionMoving();

        yield return new WaitForSecondsRealtime(followPlayerCommand.duration);

        direction = Vector3.zero;
        followPlayerCommand.isAble = false;
    }

    private IEnumerator DashToPlayer()
    {
        AddForce(directionToPlayer, dashForce);

        yield return new WaitForSeconds(dashDuration);

        dashToPlayerCommand.isAble = false;
        ResetImpact();
    }

    private IEnumerator JumpOffGround()
    {
        AddForce(Vector3.up, jumpForce);
        falling = true;

        yield return new WaitUntil(() => isGrounded && !falling);

        ResetImpactY();
    }

    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.05f;
    private void RotateInDirectionMoving()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (direction.magnitude >= 0.1f && !falling)
            transform.rotation = Quaternion.Euler(0f, smoothRotation, 0f);
    }

    #endregion

    #region Wall Movement

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

        yield return new WaitForSeconds(onWallDuration);

        onWall = false;
        falling = true;
        currentFallSpeed -= 5f;
        RandomX = 0f;
        direction = Vector3.zero;
        calculateChances = true;
    }

    private Vector3 CalculateDirectionOnWall()
    {
        if (RandomX == 0f)
            RandomX = Random.Range(-1f, 1f);

        float targetAngle = Mathf.Atan2(RandomX, 0f) * Mathf.Rad2Deg;
        Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward;
        return movementDirection;
    }

    private void JumpOffWall()
    {
        currentFallSpeed -= 5f;
        AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * new Vector3(0f, 0.5f, 1f), jumpForce * 2f);
        StopCoroutine(OnWallEnter());
        onWall = false;
        falling = true;
        RandomX = 0f;
        direction = Vector3.zero;
        calculateChances = true;
        jumpOffWallCommand.isAble = false;
    }

    #endregion

}
