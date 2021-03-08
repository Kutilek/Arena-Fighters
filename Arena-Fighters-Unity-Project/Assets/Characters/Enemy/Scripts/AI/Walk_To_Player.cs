using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_To_Player : Ability_AI
{
    protected Transform player;
    protected Vector3 directionToPlayer;
    protected float distanceToPlayer;
    [SerializeField] protected float cooldown;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        directionToPlayer = (transform.position - player.transform.position).normalized;
    }

    public override void Cast()
    {
        if (!casted)
        {
            characterPhysics.additionalSpeedMultiplier = 1.2f;
            characterPhysics.SetInputDirection(-directionToPlayer);
        }
    }

    protected override void DoStuffOnPeriodCall()
    { 
        base.DoStuffOnPeriodCall();
        if (distanceToPlayer <= 15f)
        {
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
