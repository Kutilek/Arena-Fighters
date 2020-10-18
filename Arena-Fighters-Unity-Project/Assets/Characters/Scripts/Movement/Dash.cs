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
    
    public void CastDash()
    {
        if (!dashed && characterPhysics.currentGravityState == GravityState.Grounded || characterPhysics.currentGravityState == GravityState.Falling)
            StartCoroutine(CalculateDash());
    }
    
    public bool GetDashed()
    {
        return dashed;
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

    private IEnumerator CalculateDash()
    {
        float targetAngle = Mathf.Atan2(characterPhysics.inputDirection.x, characterPhysics.inputDirection.z) * Mathf.Rad2Deg;
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle, 0f) * Vector3.forward;

        StartCoroutine(ResetDash());

        if (characterPhysics.currentGravityState == GravityState.Falling)
            dashDirection += Vector3.down;

        characterPhysics.AddForce(dashDirection, dashForce);

        yield return new WaitForSeconds(dashDuration);

        characterPhysics.ResetImpact();
    }
}
