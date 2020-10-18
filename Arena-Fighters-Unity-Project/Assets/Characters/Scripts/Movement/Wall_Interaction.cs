using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
public class Wall_Interaction : MonoBehaviour
{
    private Character_Physics characterPhysics;
    private Transform arenaCenter;
    [SerializeField] private float onWallDuration;
    [SerializeField] private float jumpOffWallForce;

    public Vector3 CalculateDirectionOnWall(Vector3 inputDirection)
    { 
        if (inputDirection.magnitude >= 0.2f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, 0f) * Mathf.Rad2Deg;
            Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward;
            return new Vector3(movementDirection.x, 0f, 0f) * 5f + Vector3.forward;
        }
        return Vector3.zero;
    }

    public void JumpOffWall()
    {
        StopCoroutine(OnWallEnter());
        characterPhysics.AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * new Vector3(0f, 0.5f, 1f), jumpOffWallForce);
        characterPhysics.currentGravityState = GravityState.JumpedOffWall;
    }

    private void Awake()
    {
        characterPhysics = GetComponent<Character_Physics>();
        arenaCenter = GameObject.FindGameObjectWithTag("ArenaCenter").transform;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall" && characterPhysics.currentGravityState == GravityState.Falling && !characterPhysics.checkForGround)
            StartCoroutine(OnWallEnter());
    }

    private IEnumerator OnWallEnter()
    {
        characterPhysics.currentGravityState = GravityState.OnWall;
        characterPhysics.ResetImpact();
        characterPhysics.ResetGravity();
        RotateOnWall();

        yield return new WaitForSeconds(onWallDuration);

        characterPhysics.currentGravityState = GravityState.Falling;
    }

    private void RotateOnWall()
    {
        transform.LookAt(arenaCenter);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    } 
}
