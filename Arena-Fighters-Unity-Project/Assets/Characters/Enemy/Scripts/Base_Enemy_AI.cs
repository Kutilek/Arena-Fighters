using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
public class Base_Enemy_AI : MonoBehaviour
{
    protected Character_Physics characterPhysics;
    protected Dash dash;
    protected Jump jump;
    protected Wall_Interaction wallInteraction;
    private Transform player;

    protected void Awake()
    {
        characterPhysics = GetComponent<Character_Physics>();
        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();
        wallInteraction = GetComponent<Wall_Interaction>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        
    }

    [SerializeField] private EnemyCommand moveRandomCommand;

  /*  private IEnumerator MoveRandom()
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
    }*/
}
