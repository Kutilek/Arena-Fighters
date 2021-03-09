using System.Collections;
using UnityEngine;

public class Walk_To_Player : Ability_AI
{
    [SerializeField] protected float cooldown;

    public override void Cast()
    {
        if (!casted)
        {
            StopCoroutine(StartCooldown());
            characterPhysics.additionalSpeedMultiplier = 1.2f;
            characterPhysics.SetInputDirection(-characterPhysics.directionToPlayer);
        }
    }

    protected override void DoStuffOnPeriodCall()
    { 
        base.DoStuffOnPeriodCall();
        if (!casted && characterPhysics.distanceToPlayer <= 9f)
        {
            characterPhysics.additionalSpeedMultiplier = 1f;
            characterPhysics.SetInputDirection(Vector3.zero);
            characterCombat.StartAttackingState();
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        casted = true;

        yield return new WaitForSeconds(cooldown);

        casted  = false;
    }
}
