using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
public class Dash : MonoBehaviour
{
    private Character_Physics characterPhysics;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashForce;
    private bool dashed;
    public bool GetDashed()
    {
        return dashed;
    }
    
    public void CastDash(Vector3 inputDirection)
    {
        if (!dashed && characterPhysics.currentGravityState == GravityState.Grounded || characterPhysics.currentGravityState == GravityState.Falling)
            StartCoroutine(CalculateDash(inputDirection));
    }
    
    private void Awake()
    {
        characterPhysics = GetComponent<Character_Physics>();
    }

    private IEnumerator ResetDash()
    {
        dashed = true;

        yield return new WaitForSeconds(dashCooldown);

        dashed = false;
    }

    private IEnumerator CalculateDash(Vector3 inputDirection)
    {
        // Calculate the angle of dash based on the Character Rotation Angle
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle, 0f) * Vector3.forward;

        // Coroutine for Dash Cooldown
        StartCoroutine(ResetDash());

        // If Character is Falling also Pushes the Character Down
        if (characterPhysics.currentGravityState == GravityState.Falling)
            dashDirection += Vector3.down;

        characterPhysics.AddForce(dashDirection, dashForce);

        // Reseting Impact After dash is done
        yield return new WaitForSeconds(dashDuration);

        characterPhysics.ResetImpact();
    }
}
