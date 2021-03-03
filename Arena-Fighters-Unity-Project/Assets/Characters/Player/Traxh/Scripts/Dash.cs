using System.Collections;
using UnityEngine;

public class Dash : Ability
{
    protected new Physics_Player characterPhysics;
    public bool ableToCastDash = true;
    private ParticleSystem ps;
    
    protected override void Start()
    {
        base.Start();
        characterPhysics = GetComponent<Physics_Player>();
        ps = transform.Find("Parciles_Dash").GetComponent<ParticleSystem>();
    }

    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashForce;

    public float GetDashCooldown()
    {
        return dashCooldown;
    }

    public bool GetDashed()
    {
        return true;
      //  return dashed;
    }

    public override void Cast()
    {
        if (ableToCastDash)
        {
            characterCombat.CheckIfAttackign();
            ableToCastDash = false;
            characterCombat.ableToCast = false;
            animator.SetTrigger(animationCondition);
            characterCombat.attacking = false;
            ps.Play();
        }     
    }

    // Called from Animation Event
    private void DashOnGround()
    {
        StartCoroutine(CalculateDash(characterPhysics.inputDirectionRaw));
    }

    private void DashInFall()
    {
        CalculateDashInAir(characterPhysics.inputDirectionRaw);
    }

    private void SetAbleToCastDashTrue()
    {
        ableToCastDash = true;
    }

    private void SetAbleToCastDashFalse()
    {
        ableToCastDash = false;
    }

    private void CalculateDashInAir(Vector3 inputDirection)
    {
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle, 0f) * Vector3.forward;

        dashDirection += Vector3.down;

        characterPhysics.AddForce(dashDirection, dashForce);
    }

    private IEnumerator CalculateDash(Vector3 inputDirection)
    {
        // Calculate the angle of dash based on the Character Rotation Angle
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        Vector3 dashDirection = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle, 0f) * Vector3.forward;

        // Coroutine for Dash Cooldown
       // StartCoroutine(ResetDash());

        characterPhysics.AddForce(dashDirection, dashForce);

        // Reseting Impact After dash is done
        yield return new WaitForSeconds(dashDuration);

        characterPhysics.ResetImpact();
        animator.SetTrigger("dashEnd");     
    }
}
