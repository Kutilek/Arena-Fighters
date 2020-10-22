using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
public class Wall_Interaction : MonoBehaviour
{
    private Character_Physics characterPhysics;

    //Arena Center Transform where character will rotate
    private Transform arenaCenter;
    [SerializeField] private float onWallDuration;
    [SerializeField] private float jumpOffWallForce;

    // If the inputDirection.x is greater than 0.2f calculate Vector3 for the movement on wall
    public Vector3 CalculateDirectiOnOnWall(Vector3 inputDirection)
    {
        if (Mathf.Abs(inputDirection.x) >= 0.2f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, 0f) * Mathf.Rad2Deg;
            Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y - targetAngle, 0f) * Vector3.forward;
            return new Vector3(movementDirection.x, 0f, 0f) * 5f + Vector3.forward;
        }
        return Vector3.zero;
    }

    public void JumpOffWall()
    {
        // Stops All Coroutines Just in Case OnWallEnter() has not stopped
        StopAllCoroutines();
        characterPhysics.AddForce(Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * new Vector3(0f, 1f, 1f), jumpOffWallForce);
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
        // Make Player Stick to the wall
        characterPhysics.currentGravityState = GravityState.OnWall;
        characterPhysics.ResetGravity();
        StartCoroutine(FallFromWall());
        RotateToArenaCenter();

        // After 0.2f Reseting Impact so the player won't slide up the wall
        yield return new WaitForSeconds(0.2f);

        characterPhysics.ResetImpact();
    }

    // Coroutine that will make the player fall from fall after some time
    private IEnumerator FallFromWall()
    {
        yield return new WaitForSeconds(onWallDuration);

        characterPhysics.currentGravityState = GravityState.Falling;
    }

    private void RotateToArenaCenter()
    {
        transform.LookAt(arenaCenter);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
